using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BootCamp.Core.Services.Interfaces;
using BootCamp.DataLayer.Context;
using BootCamp.DataLayer.Entities.Course;

namespace BootCamp.Core.Services
{
    public class CourseService:ICourseService
    {
        private ApplicationDbContext _context;

        public CourseService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CourseGroup> GetAllGroup()
        {
            return _context.CourseGroup.ToList();
        }
    }
}
