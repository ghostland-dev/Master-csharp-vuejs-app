using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MasterCurrencyData.Models;

namespace MasterCurrencyData.Data
{
    public class MasterCurrencyDataContext : DbContext
    {
        public MasterCurrencyDataContext (DbContextOptions<MasterCurrencyDataContext> options)
            : base(options)
        {
        }

        public DbSet<MasterCurrencyData.Models.Currency> Currency { get; set; } = default!;
    }
}
