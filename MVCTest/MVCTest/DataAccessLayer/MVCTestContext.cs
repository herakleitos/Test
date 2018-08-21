using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MVCTest.Models;
using MVCTest.Config;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCTest.DataAccessLayer
{
    public class MVCTestContext: DbContext
    {
        public MVCTestContext() : base(WebConfig.DataBaseConnectionString())
        {
        }
        public DbSet<Employee> Employees { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().ToTable("T_BASE_EMPLOYEE").Property(p=>p.EmployeeId)
            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            base.OnModelCreating(modelBuilder);
        }
    }
}