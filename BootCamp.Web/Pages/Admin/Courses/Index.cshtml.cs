using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;

namespace BootCamp.Web.Pages.Admin.Courses
{
    [PermissionChecker(10)]

    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}