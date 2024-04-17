using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APIRFID.Model;

namespace APIRFID.Data
{
    public class APIRFIDContext : DbContext
    {
        public APIRFIDContext (DbContextOptions<APIRFIDContext> options)
            : base(options)
        {
        }

        public DbSet<APIRFID.Model.User> User { get; set; } = default!;

        public DbSet<APIRFID.Model.UserE>? UserE { get; set; }
    }
}
