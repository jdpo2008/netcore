using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Tgs.DataAccess.Core.Context;
using Tgs.DataContract.Identity;
using Tgs.Entities.Seguridad;

namespace Tgs.Web.UI.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext Context)
        {
            _context = Context;
        }

        [BindProperty]
        public UserModelDto Input { get; set; }

        public List<UserModelDto> Users { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Users = await _context.Users.Select(user => new UserModelDto 
            { 
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed
                //Roles = _userManager.GetRolesAsync(user)
            }).ToListAsync();

            return Page();
        }
    }
}
