using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBILL.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UsedProductKey> UsedProductKeys { get; set; }
        public DbSet<CustUser> CustUsers { get; set; }
        public DbSet<CustParty> CustPartys { get; set; }
        public DbSet<YearAccount> YearAccounts { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ProcessItem> ProcessItems { get; set; }
        public DbSet<PartyChallan> PartyChallans { get; set; }
        public DbSet<PartyChallanItem> PartyChallanItems { get; set; }

        public AppDbContext() : base("name=AppDbContext")
        {
        }
    }
}
