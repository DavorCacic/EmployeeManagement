using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    //Initialy, this class was set to derive from DbContext class. However, in order to implement out-of-the-box Entity framework login functionality, it was changed to derive from IdentityDbContext.
    //Steps for implementation:
    //1. Make custom class derive from IdentityDbContext class, 
    //2. Add service (service.AddIdentity<>()),
    //3. Add midleware to pipeline ()
    //4. Create migration (Note that base.OnModelCreating(modelBuilder) was added to OnModelCreating overridden method) ,
    //5. update database
    public class AppDbContex : IdentityDbContext
    {
        //One property for all classes that will be stored in DB
        public DbSet<Employee> Employees { get; set; }

        public AppDbContex(DbContextOptions<AppDbContex> options) : base(options)
        {

        }

        //This is for populating dummy data into DB. Method Seed is defined by the user (in ModelBuilderExtensionClass)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Seed();
        }


    }
}
