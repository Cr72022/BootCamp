using System;
using System.Collections.Generic;
using System.Text;
using BootCamp.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BootCamp.Core.Services.Interfaces
{
    public interface ICourseService
    {

        #region Group

        List<CourseGroup> GetAllGroup();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetLevels();
        List<SelectListItem> GetStatues();


        #endregion

    }
}
