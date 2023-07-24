using BootCamp.Core.DTOs;
using BootCamp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;

namespace BootCamp.Web.Pages.Admin.User
{
    [PermissionChecker(4)]

    public class DeleteUserModel : PageModel
    {
        private IAccountService _userService;

        public DeleteUserModel(IAccountService userService)
        {
            _userService = userService;
        }

        public MainPanelViewModel InformationUserViewModel { get; set; }
        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            InformationUserViewModel = _userService.GetInformationForUserPanel(id);
        }

        public IActionResult OnPost(int UserId)
        {
            _userService.DeleteUser(UserId);
            return RedirectToPage("Index");
        }
    }
}
