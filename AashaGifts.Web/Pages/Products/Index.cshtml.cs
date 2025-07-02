using AashaGifts.Web.Data;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AashaGifts.Web.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;
        public IndexModel(AppDbContext db) { _db = db; }
        public List<Product> Products { get; set; } = new();

        public void OnGet()
        {
            Products = _db.Products.Select(p => new Product
            {
                Id = p.Id,
                Name = p.Name,
                Category = p.Category,
                Description = p.Description,
                Price = p.Price,
                Images = p.Images.ToList()
            }).ToList();
        }
    }
}