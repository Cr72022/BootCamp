using System;
using System.Collections.Generic;
using System.Text;
using BootCamp.DataLayer.Entities.Course;

namespace BootCamp.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region Group

        List<CourseGroup> GetAllGroup();

        #endregion
    }
}
