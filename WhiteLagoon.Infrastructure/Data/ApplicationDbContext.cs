using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLagoon.Infrastructure.Data {
    public class ApplicationDbContext : DbContext {
        //the options instance with :base(options) passes all parameters from ApplicationDbContext to it's base class DbContext
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){
            
        }
    }
}
