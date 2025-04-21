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

        public AppDbContext() : base("name=AppDbContext")
        {
        }
    }
}
