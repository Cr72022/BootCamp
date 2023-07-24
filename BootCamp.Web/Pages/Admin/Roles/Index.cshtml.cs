using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BootCamp.Core.DTOs;
using BootCamp.Core.Services.Interfaces;
using BootCamp.DataLayer.Entities.User;
using TopLearn.Core.Security;

namespace BootCamp.Web.Pages.Admin.Roles
{
    [PermissionChecker(5)]

    public class IndexModel : PageModel
    {
        private IPermissionService _permissionService;

        public IndexModel(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        public List<Role> RolesList { get; set; }


        public void OnGet()
        {
            RolesList = _permissionService.GetRoles();
        }

       
    }
}