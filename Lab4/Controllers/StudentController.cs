using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Models.ViewModel;

namespace Lab4.Controllers
{
    public class StudentController : Controller
    {
        private readonly SchoolCommunityContext _context;

        public StudentController(SchoolCommunityContext context)
        {
            _context = context;
        }

        // GET: Student
        public async Task<IActionResult> Index(int? ID)
        {
            var viewModel = new CommunityViewModel();
            viewModel.Students = await _context.Students
                .Include(c => c.CommunityMemberships)
                .ThenInclude(s => s.Community)
                .OrderBy(c => c.FirstName)
                .AsNoTracking()
                .ToListAsync();

        if (ID != null)
        {
             ViewData["CommunityID"] = ID;
             viewModel.CommunityMemberships = viewModel.Students.Where(x => x.ID == ID).Single()
                    .CommunityMemberships;
            }

            return View(viewModel);
        }

        // GET: Student/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Student/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            if (id != student.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        public async Task<IActionResult> Membership(int? id, string communityID)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stu = _context.Students.Where(x => x.ID == id).Single();

            var data =
                from c in _context.Communities orderby c.Title
                select new CommunityMembersipViewModel()
                {
                    CommunityId = c.ID,
                    Title = c.Title,
                    IsMember = _context.Students.Where(x => x.ID == id).Single()
                    .CommunityMemberships.Any(x => x.CommunityID == c.ID)                  
                };

            if (communityID != null)
            {
                var exist = data.Where(x => x.CommunityId == communityID).Single().IsMember;
                if (exist == true)
                {
                    var membership = _context.CommunityMemberships
                        .Where(x => x.CommunityID == communityID)
                        .Where(x => x.StudentID == id)
                        .Single();
                    _context.CommunityMemberships.Remove(membership);
                    _context.SaveChanges();
                }
                else
                {
                    _context.CommunityMemberships.Add(new CommunityMembership()
                    {
                        Student=stu,
                        StudentID=stu.ID,
                        CommunityID=communityID,
                        Community=_context.Communities.Where(x=>x.ID== communityID).Single()
                    });
                    _context.SaveChanges();
                }

                data =
                from c in _context.Communities orderby c.Title
                select new CommunityMembersipViewModel()
                {
                    CommunityId = c.ID,
                    Title = c.Title,
                    IsMember = _context.Students.Where(x => x.ID == id).Single()
                    .CommunityMemberships.Any(x => x.CommunityID == c.ID)
                };

            }

            
            var cas = new StudentMembershipViewModel()
            {
                Student = stu,
                Memberships = await data.OrderByDescending(x => x.IsMember).AsNoTracking().ToListAsync()
        };

            return View(cas);
        }
        

        // GET: Student/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.ID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.ID == id);
        }
    }
}
