using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity;

namespace Week12.Controllers
{
    
    public class ToetsResultaatController : Controller
    {
        private readonly MyContext _context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityRole> userManager;

        public ToetsResultaatController(MyContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityRole> userManager )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _context = context;
        }

        // GET: ToetsResultaat
        public async Task<IActionResult> Index()
        {
            return View(await _context.ToetsResultaat.ToListAsync());
        }

        // GET: ToetsResultaat/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toetsResultaat = await _context.ToetsResultaat
                .FirstOrDefaultAsync(m => m.id == id);
            if (toetsResultaat == null)
            {
                return NotFound();
            }

            return View(toetsResultaat);
        }
    
        // GET: ToetsResultaat/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: ToetsResultaat/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,StudentNaam,Cijfer")] ToetsResultaat toetsResultaat)
        {
            if (ModelState.IsValid)
            {
                _context.Add(toetsResultaat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(toetsResultaat);
        }

        // GET: ToetsResultaat/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toetsResultaat = await _context.ToetsResultaat.FindAsync(id);
            if (toetsResultaat == null)
            {
                return NotFound();
            }
            return View(toetsResultaat);
        }

        // POST: ToetsResultaat/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,StudentNaam,Cijfer")] ToetsResultaat toetsResultaat)
        {
            if (id != toetsResultaat.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(toetsResultaat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToetsResultaatExists(toetsResultaat.id))
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
            return View(toetsResultaat);
        }

        // GET: ToetsResultaat/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var toetsResultaat = await _context.ToetsResultaat
                .FirstOrDefaultAsync(m => m.id == id);
            if (toetsResultaat == null)
            {
                return NotFound();
            }

            return View(toetsResultaat);
        }

        // POST: ToetsResultaat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var toetsResultaat = await _context.ToetsResultaat.FindAsync(id);
            _context.ToetsResultaat.Remove(toetsResultaat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet("login")]
        public IActionResult Login(string returnUrl )
        {
           ViewData["ReturnUrl"]= returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string naam, string password, string returnUrl)
        {
            if(naam=="younes" && password=="123")
            {
                var claims = new List <Claim>();
                claims.Add(new Claim("naam", naam));
                claims.Add(new Claim(ClaimTypes.NameIdentifier , naam));
                var ClaimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var cp = new ClaimsPrincipal(ClaimsIdentity);
                await HttpContext.SignInAsync(cp);
                return Redirect(returnUrl);
                
            }
            return BadRequest();
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }


        private bool ToetsResultaatExists(int id)
        {
            return _context.ToetsResultaat.Any(e => e.id == id);
        }
    }
}
