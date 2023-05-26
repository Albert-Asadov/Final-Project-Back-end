using System;
using Final_Project_Razor.DAL;
using Final_Project_Razor.Entities;

namespace Razor_Final_Project_Code_Academy.Service
{
	public class LayoutService
	{
        readonly RazorDbContext _context;
        readonly IHttpContextAccessor _accessor;

        public LayoutService(RazorDbContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            _accessor = accessor;
        }

        public List<Category> AllCategories()
        {
            List<Category> categories = _context.Categories.ToList();

            return categories;
        }
    }
}

