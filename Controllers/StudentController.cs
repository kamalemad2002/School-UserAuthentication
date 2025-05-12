using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using School.Data;
using School.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using School.Data;

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
                    PhoneNumber = viewModel.PhoneNumber,
                    Email = viewModel.Email,
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
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var roule = HttpContext.Session.GetString("Roule");
            if (roule == "Admin")
            {
                if (id == null)
                {
                    return NotFound();
                }
                var matchs = await dbcontext.students.FindAsync(id);

                if (matchs == null)
                {
                    return NotFound();
                }
                return View(matchs);
            }

            else

            {
                return Content("Access Denied");
            }

        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var st = await dbcontext.students.FindAsync(id);
            if (st != null)
            {
                dbcontext.students.Remove(st);
            }

            await dbcontext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



    }

    internal class CustomAuthorizationFilterAttribute : Attribute
    {
    }
}
