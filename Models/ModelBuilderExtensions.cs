using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                 new Employee() { Id = 1, Name = "Caslav", Email = "caslav@osvezenje.com", Department = Department.IT },
                new Employee() { Id = 2, Name = "Vanja", Email = "vanjuka@osvezenje.com", Department = Department.Design },
                new Employee() { Id = 3, Name = "Fedja", Email = "fedjika@osvezenje.com", Department = Department.Accounting },
                new Employee() { Id = 4, Name = "Oksana", Email = "oko@osvezenje.com", Department = Department.HR },
                new Employee() { Id = 5, Name = "Kobrim", Email = "cobra@osvezenje.com", Department = Department.Development },
                new Employee() { Id = 6, Name = "Jekara", Email = "jk@osvezenje.com", Department = Department.Testing }
                );
        }
    }
}
