using AashaGifts.Web.Data;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AashaGifts.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) { _db = db; }
        public List<Product> Products { get; set; } = new();

        public void OnGet()
        {
            Products = _db.Products.Include(p => p.Images).ToList();
        }
    }
}