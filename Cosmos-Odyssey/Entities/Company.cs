﻿
using System.ComponentModel.DataAnnotations;

namespace Cosmos_Odyssey.Entities
{
    public class Company
    {
        [Key]
        public string Id { get; set; }
        
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
