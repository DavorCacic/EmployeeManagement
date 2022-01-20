using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>()
            {
                new Employee() { Id = 1, Name = "Caslav", Email = "caslav@osvezenje.com", Department = Department.IT}, 
                new Employee() { Id = 2, Name = "Vanja", Email = "vanjuka@osvezenje.com", Department = Department.Design}, 
                new Employee() { Id = 3, Name = "Fedja", Email = "fedjika@osvezenje.com", Department = Department.Accounting}, 
                new Employee() { Id = 4, Name = "Oksana", Email = "oko@osvezenje.com", Department = Department.HR}, 
                new Employee() { Id = 5, Name = "Kobrim", Email = "cobra@osvezenje.com", Department = Department.Development}, 
                new Employee() { Id = 6, Name = "Jekara", Email = "jk@osvezenje.com", Department = Department.Testing }
            };
        }

        public Employee Add(Employee employee)
        {
           employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }
    }
}
