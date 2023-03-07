using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

using System.Security.Claims;

namespace SecurityForAssessmentStudent.Pages.ClaimsManager
{
    public class AssignModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AssignModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public SelectList Users { get; set; }
        [BindProperty, Required]
        public string SelectedUserId { get; set; }
        [BindProperty, Required]
        public string ClaimType { get; set; }
        [BindProperty]
        public string? ClaimValue { get; set; }
        public async Task OnGetAsync()
        {
            await GetOptions();
        }
        public async Task GetOptions()
        {
            var users = await _userManager.Users.ToListAsync();
            Users = new SelectList(users, nameof(IdentityUser.Id), nameof(IdentityUser.UserName));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var claim = new Claim(ClaimType, ClaimValue ?? String.Empty);
                var user = await _userManager.FindByIdAsync(SelectedUserId);
                await _userManager.AddClaimAsync(user, claim);
                return RedirectToPage("/ClaimsManager/Index");
            }
            await GetOptions(); return Page();
        }


    }
}
