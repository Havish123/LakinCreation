using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AashaGifts.Web.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly AashaGifts.Web.Data.AppDbContext _context;

        public IndexModel(AashaGifts.Web.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}
