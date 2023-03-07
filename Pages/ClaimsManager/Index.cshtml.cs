using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace SecurityForAssessmentStudent.Pages.ClaimsManager
{
    public class IndexModel : PageModel
    {
        public UserManager<IdentityUser> UserManager;

        public IndexModel(UserManager<IdentityUser> userManager)
        {
            UserManager = userManager;
        }
        public List<IdentityUser> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await UserManager.Users.ToListAsync();
        }
    }
}
