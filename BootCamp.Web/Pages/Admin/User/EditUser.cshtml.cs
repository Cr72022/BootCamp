using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootCamp.Core.DTOs;
using BootCamp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BootCamp.Core.DTOs;
using BootCamp.Core.Services.Interfaces;
using TopLearn.Core.Security;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(3)]

    public class EditUserModel : PageModel
    {
        private IAccountService _userService;
        private IPermissionService _permissionService;

        public EditUserModel(IAccountService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }




        [BindProperty]
        public EditUserViewModel EditUserViewModel { get; set; }
        public void OnGet(int id)
        {
            EditUserViewModel = _userService.GetUserForShowInEditMode(id);
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
           
            _userService.EditUserFromAdmin(EditUserViewModel);

            //Edit Roles
            _permissionService.EditRolesUser(EditUserViewModel.UserId, SelectedRoles);
            return RedirectToPage("Index");
        }
    }
}