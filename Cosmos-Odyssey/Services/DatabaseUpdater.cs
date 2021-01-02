using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Cosmos_Odyssey.Data;
using System;
using Microsoft.Extensions.DependencyInjection;
using Cosmos_Odyssey.Entities;
using Microsoft.EntityFrameworkCore;
using System.Timers;
using System.Linq;
using Microsoft.Extensions.Options;
using Cosmos_Odyssey.Helpers;

namespace Cosmos_Odyssey.Services
{
    public class DatabaseUpdater : IDatabaseUpdater
    {
        private readonly ILogger<DatabaseUpdater> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<AppSettings> _appSettings;

        public DatabaseUpdater(ILogger<DatabaseUpdater> logger, IServiceScopeFactory scopeFactory, IServiceProvider serviceProvider, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
        }

        public async void StartAsync()
        {
            await UpdateDatabaseAsync();

            var timer = new Timer(_appSettings.Value.DatabaseUpdateIntervalSeconds * 1000)
            {
                AutoReset = true
            };
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await UpdateDatabaseAsync();
        }

        private async Task UpdateDatabaseAsync()
        {
            try
            {
                var scope = _scopeFactory.CreateScope();
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                await RemoveOldPricelistsAsync(databaseContext);
                await RemoveExpiredReservationsAsync(databaseContext);
                await AddNewPriceListAsync(databaseContext);

                await databaseContext.DisposeAsync();
            }
            catch (Exception exception)
            {
                _logger.LogCritical($"Failed to update database: {exception.Message}");
            }
        }

        private async Task AddNewPriceListAsync(DatabaseContext databaseContext)
        {
            var apiService = _serviceProvider.GetService(typeof(IApiService)) as IApiService;

            if (apiService == null)
                return;

            //Check if pricelist expired:
            if (databaseContext.PriceList.Any(x => x.ValidUntil > DateTime.Now))
                return;

            //Load new one:
            var apiPriceList = await apiService.GetPriceListAsync();

            if (apiPriceList == null)
                return;

            await AddPriceListAsync(databaseContext, apiPriceList);

            foreach (var apiLeg in apiPriceList.Legs)
            {
                await AddFromAsync(databaseContext, apiLeg.RouteInfo.From);
                await AddToAsync(databaseContext, apiLeg.RouteInfo.To);
                await AddRouteInfoAsync(databaseContext, apiLeg.RouteInfo, apiLeg);
                await AddLegAsync(databaseContext, apiLeg, apiPriceList.Id);

                foreach (var apiProvider in apiLeg.Providers)
                {
                    await AddCompanyAsync(databaseContext, apiProvider);
                    await AddProviderAsync(databaseContext, apiProvider, apiLeg.Id);
                }
            }

            //Ready new pricelist:
            var newPriceList = await databaseContext.PriceList.FirstOrDefaultAsync(x => x.Id == apiPriceList.Id);
            newPriceList.Ready = true;
            await databaseContext.SaveChangesAsync();
        }

        private async Task AddPriceListAsync(DatabaseContext databaseContext, Pricelist apiPricelist)
        {
            if (await databaseContext.PriceList.AnyAsync(x => x.Id == apiPricelist.Id))
                return;

            await databaseContext.PriceList.AddAsync(new Pricelist
            {
                Id = apiPricelist.Id,
                ValidUntil = apiPricelist.ValidUntil.ToLocalTime()
            });
            await databaseContext.SaveChangesAsync();
        }

        private async Task AddFromAsync(DatabaseContext databaseContext, From apiFrom)
        {
            if (await databaseContext.From.AnyAsync(x => x.Id == apiFrom.Id))
                return;

            await databaseContext.From.AddAsync(apiFrom);
            await databaseContext.SaveChangesAsync();
        }

        private async Task AddToAsync(DatabaseContext databaseContext, To apiTo)
        {
            if (await databaseContext.To.AnyAsync(x => x.Id == apiTo.Id))
                return;

            await databaseContext.To.AddAsync(apiTo);
            await databaseContext.SaveChangesAsync();
        }

        private async Task AddRouteInfoAsync(DatabaseContext databaseContext, RouteInfo apiRouteInfo, Leg apiLeg)
        {
            if (await databaseContext.RouteInfo.AnyAsync(x => x.Id == apiRouteInfo.Id))
                return;

            await databaseContext.RouteInfo.AddAsync(new RouteInfo()
            {
                Id = apiLeg.RouteInfo.Id,
                Distance = apiLeg.RouteInfo.Distance,
                FromId = apiLeg.RouteInfo.From.Id,
                ToId = apiLeg.RouteInfo.To.Id
            });
            await databaseContext.SaveChangesAsync();
        }

        private async Task AddLegAsync(DatabaseContext databaseContext, Leg apiLeg, string priceListId)
        {
            if (await databaseContext.Leg.AnyAsync(x => x.Id == apiLeg.Id))
                return;

            await databaseContext.Leg.AddAsync(new Leg
            {
                Id = apiLeg.Id,
                PriceListId = priceListId,
                RouteInfoId = apiLeg.RouteInfo.Id
            });
            await databaseContext.SaveChangesAsync();
        }

        private async Task AddCompanyAsync(DatabaseContext databaseContext, Provider apiProvider)
        {
            if (await databaseContext.Company.AnyAsync(x => x.Id == apiProvider.Company.Id))
                return;

            await databaseContext.Company.AddAsync(new Company()
            {
                Id = apiProvider.Company.Id,
                Name = apiProvider.Company.Name
            });
            await databaseContext.SaveChangesAsync();
        }

        private async Task AddProviderAsync(DatabaseContext databaseContext, Provider apiProvider, string legId)
        {
            if (await databaseContext.Provider.AnyAsync(x => x.Id == apiProvider.Id))
                return;

            await databaseContext.Provider.AddAsync(new Provider
            {
                Id = apiProvider.Id,
                FlightEnd = apiProvider.FlightEnd.ToLocalTime(),
                FlightStart = apiProvider.FlightStart.ToLocalTime(),
                CompanyId = apiProvider.Company.Id,
                Price = apiProvider.Price,
                LegId = legId
            });
            await databaseContext.SaveChangesAsync();
        }

        private async Task RemoveOldPricelistsAsync(DatabaseContext databaseContext)
        {
            if (databaseContext.PriceList.Count() <= _appSettings.Value.MaxPriceListCount)
                return;

            var priceList = await databaseContext.PriceList
                    .Include(x => x.Legs)
                    .ThenInclude(x => x.RouteInfo)
                    .ThenInclude(x => x.To)
                    .Include(x => x.Legs)
                    .ThenInclude(x => x.RouteInfo)
                    .ThenInclude(x => x.From)
                    .Include(x => x.Legs)
                    .ThenInclude(x => x.Providers)
                    .ThenInclude(x => x.Company)
                    .OrderBy(x => x.ValidUntil).FirstOrDefaultAsync();

            priceList.Legs.ToList().ForEach(l => databaseContext.To.Remove(l.RouteInfo.To));
            priceList.Legs.ToList().ForEach(l => databaseContext.From.Remove(l.RouteInfo.From));
            priceList.Legs.ToList().ForEach(l => databaseContext.RouteInfo.Remove(l.RouteInfo));
            priceList.Legs.ToList().ForEach(l => l.Providers.ToList().ForEach(p =>
            {
                if (p.Company != null)
                {
                    databaseContext.Company.Remove(p.Company);
                }
            }));
            priceList.Legs.ToList().ForEach(l => databaseContext.Provider.RemoveRange(l.Providers));
            databaseContext.Leg.RemoveRange(priceList.Legs);
            databaseContext.PriceList.Remove(priceList);
            await databaseContext.SaveChangesAsync();

        }

        private async Task RemoveExpiredReservationsAsync(DatabaseContext databaseContext)
        {
            var reservations = await databaseContext.Reservation
                .Where(x => x.Provider.Leg.Pricelist.ValidUntil < DateTime.Now)
                .ToListAsync();

            databaseContext.Reservation.RemoveRange(reservations);
            await databaseContext.SaveChangesAsync();
        }
    }
}
