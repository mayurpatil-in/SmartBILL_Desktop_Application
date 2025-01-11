using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBILL.Models
{
    public class RegistrationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UsedProductKey> UsedProductKeys { get; set; }

        public RegistrationContext() : base("name=AppDbContext")
        {
        }
    }
}
