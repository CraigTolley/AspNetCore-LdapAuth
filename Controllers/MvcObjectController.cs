using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestAuth2Mvc.Models;

namespace TestAuth2Mvc.Controllers
{
    public class MvcObjectController : Controller
    {
        private readonly TestMvcContext _context;

        public MvcObjectController(TestMvcContext context)
        {
            _context = context;
        }

        // GET: MvcObject
        public async Task<IActionResult> Index()
        {
            return View(await _context.Object.ToListAsync());
        }

        // GET: MvcObject/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mvcObject = await _context.Object
                .FirstOrDefaultAsync(m => m.MvcObjectId == id);
            if (mvcObject == null)
            {
                return NotFound();
            }

            return View(mvcObject);
        }

        // GET: MvcObject/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MvcObject/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MvcObjectId,MvcObjectName")] MvcObject mvcObject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mvcObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mvcObject);
        }

        // GET: MvcObject/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mvcObject = await _context.Object.FindAsync(id);
            if (mvcObject == null)
            {
                return NotFound();
            }
            return View(mvcObject);
        }

        // POST: MvcObject/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MvcObjectId,MvcObjectName")] MvcObject mvcObject)
        {
            if (id != mvcObject.MvcObjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mvcObject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MvcObjectExists(mvcObject.MvcObjectId))
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
            return View(mvcObject);
        }

        // GET: MvcObject/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mvcObject = await _context.Object
                .FirstOrDefaultAsync(m => m.MvcObjectId == id);
            if (mvcObject == null)
            {
                return NotFound();
            }

            return View(mvcObject);
        }

        // POST: MvcObject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mvcObject = await _context.Object.FindAsync(id);
            _context.Object.Remove(mvcObject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MvcObjectExists(int id)
        {
            return _context.Object.Any(e => e.MvcObjectId == id);
        }
    }
}
