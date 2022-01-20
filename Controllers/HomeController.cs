using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Controllers
{
    //In order to return JSON data, or View, this class must inherit from Controller class. Otherwise, it can only return types like string, object, etc. 
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        public IHostEnvironment HostEnvironment { get; }

        public HomeController(IEmployeeRepository employeeRepository, IHostEnvironment hostEnvironment)
        {
            _employeeRepository = employeeRepository;
            HostEnvironment = hostEnvironment;
        }

        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();
            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Details(int? id)
        {
            Employee employee = _employeeRepository.GetEmployee(id.Value);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id.Value);
            }

            HomeDetailsViewModel homeDetailsViewModel = new()
            { 
                Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new()
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee updateEmployee = _employeeRepository.GetEmployee(model.Id);
                updateEmployee.Name = model.Name;
                updateEmployee.Department = model.Department;
                updateEmployee.Email = model.Email;
                if (model.Photo != null) 
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string oldPhoto = Path.Combine(HostEnvironment.ContentRootPath, "wwwroot\\images", model.ExistingPhotoPath);
                        System.IO.File.Delete(oldPhoto);
                    }

                    updateEmployee.PhotoPath = ProcessUploadingPhoto(model);
                }

                _employeeRepository.Update(updateEmployee);
 
                return RedirectToAction("index");
            }
            return View();

        }

       

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadingPhoto(model);

                Employee newEmployee = new()
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);

                //anonymous object, because Details now expects Struct, not intiger. This is because Details uses nullable int. 
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();

        }

        private string ProcessUploadingPhoto(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(HostEnvironment.ContentRootPath, "wwwroot\\images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                model.Photo.CopyTo(fileStream);

            }

            return uniqueFileName;
        }
    }
}
 