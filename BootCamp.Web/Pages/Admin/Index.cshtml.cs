using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;

namespace BootCamp.Web.Pages.Admin
{
    [PermissionChecker(1)]

    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
