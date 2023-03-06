using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SecurityForAssessmentStudent.Data;
using SecurityForAssessmentStudent.DTO;

namespace SecurityForAssessmentStudent.Pages.RoleManager
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public List<IdentityRole> Roles { get; set; }

        public List<UserRoles> UsersAndRoles { get; set; }
        //create the Users and Roles from the DB
        public List<UserRoles> GetUserAndRoles()
        {
            var list = (from user in _context.Users
                        join userRoles in _context.UserRoles on user.Id equals userRoles.UserId
                        join role in _context.Roles on userRoles.RoleId equals role.Id
                        select new UserRoles { UserName = user.UserName, RoleName = role.Name }).ToList();
            return list;
        }

            public void OnGet()
        {
            Roles = _roleManager.Roles.ToList();

            UsersAndRoles = GetUserAndRoles();


        }
    }
}
