using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BootCamp.Core.Services.Interfaces;
using BootCamp.DataLayer.Entities.Course;
using TopLearn.Core.Security;

namespace BootCamp.Web.Pages.Admin.Courses
{
    [PermissionChecker(11)]

    public class CreateCourseModel : PageModel
    {
        private ICourseService _courseService;

        public CreateCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public void OnGet()
        {
            var groups = _courseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(groups,"Value","Text");

            var subGrous = _courseService.GetSubGroupForManageCourse(int.Parse(groups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGrous, "Value", "Text");

            var teachers = _courseService.GetTeachers();
            ViewData["Teachers"]=new SelectList(teachers,"Value","Text");

            var levels = _courseService.GetLevels();
            ViewData["Levels"] = new SelectList(levels, "Value", "Text");

            var statues = _courseService.GetStatues();
            ViewData["Statues"] = new SelectList(statues, "Value", "Text");
        }
    }
}