using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School.Data;
using School.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace School.Controllers
{

    public class StudentController : Controller
    {
        private readonly ApplicationDbContext dbcontext;
        public StudentController(ApplicationDbContext dbcontext) 
        {
            this.dbcontext = dbcontext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                var student = new Student
                {
                    Name = viewModel.Name,
                    Email = viewModel.Email,
                    PhoneNumber = viewModel.PhoneNumber,
                    //Role = viewModel.Role,
                    governorate = viewModel.governorate
                };
                await dbcontext.students.AddAsync(student);
                await dbcontext.SaveChangesAsync();
                return RedirectToAction("List", "Student");
            }
            else
            {
                return View(viewModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var student = await dbcontext.students.ToListAsync();
            return View(student);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var st= await dbcontext.students.FirstOrDefaultAsync(kamal=>kamal.Id ==id);
            return View(st);
        }
        [HttpPost]
        
        public async Task<IActionResult> Edit(Student st)
        {
            if (ModelState.IsValid)
            {
                var stud = await dbcontext.students.FindAsync(st.Id);
                if (stud is not null)
                {
                    stud.Name = st.Name;
                    stud.PhoneNumber = st.PhoneNumber;
                    stud.Email = st.Email;
                    stud.governorate = st.governorate;
                    //stud.Role = st.RoleIsAdmin;
                    //stud.user=st.user;
                    await dbcontext.SaveChangesAsync();

                }
                return RedirectToAction("List", "Student");

            }
            else
            {
                return View(st);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            // Validate if the current user is an admin
            var currentUser = await dbcontext.users.FirstOrDefaultAsync(u => u.Id == id);
            if (currentUser == null || !currentUser.isAdmin)
            {
                return Content("Access Denied: You do not have permission to delete students.");
            }

            // Find the student to delete
            var student = await dbcontext.students.FirstOrDefaultAsync(u => u.Id == id);
            if (student == null)
            {
                return Content("Error: Student not found.");
            }

            // Delete the student
            dbcontext.students.Remove(student);
            await dbcontext.SaveChangesAsync();

            // Redirect to the student list
            return RedirectToAction("List", "Student");
        }



    }
}
