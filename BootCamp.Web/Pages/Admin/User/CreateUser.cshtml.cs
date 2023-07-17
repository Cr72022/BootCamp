using BootCamp.Core.DTOs;
using BootCamp.Core.Services;
using BootCamp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;

namespace BootCamp.Web.Pages.Admin.User
{
    [PermissionChecker(3)]

    public class CreateUserModel : PageModel
    {
        private IAccountService _accountService;
        private IPermissionService _permissionService;

        public CreateUserModel(IAccountService accountService, IPermissionService permissionService)
        {
            _accountService = accountService;
            _permissionService = permissionService;
        }


        [BindProperty]
        public CreateUserViewModel CreateUserViewModel { get; set; }

        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            //if (!ModelState.IsValid)
            //    return Page();

            int userId = _accountService.AddUserFromAdmin(CreateUserViewModel);

            //Add Roles
            _permissionService.AddRolesToUser(SelectedRoles, userId);


            return Redirect("/Admin/User");

        }
    }
}
