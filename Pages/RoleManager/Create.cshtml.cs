using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SecurityForAssessmentStudent.Pages.RoleManager
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private RoleManager<IdentityRole> _roleManager;
                public CreateModel(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

       public string Name { get; set; }
       
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                Name= Name.Trim();

                var role = new IdentityRole(Name);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToPage("/RoleManager/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Page();
        }
    }
}
