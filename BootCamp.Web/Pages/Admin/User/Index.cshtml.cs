using BootCamp.Core.DTOs;
using BootCamp.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;

namespace BootCamp.Web.Pages.Admin.User
{
    [PermissionChecker(2)]

    public class IndexModel : PageModel
    {
        private IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UserForAdminViewModel = _accountService.GetUsers(pageId, filterEmail, filterUserName);
        }
    }
}
