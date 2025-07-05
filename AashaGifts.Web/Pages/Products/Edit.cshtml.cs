using AashaGifts.Web.Data;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AashaGifts.Web.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public SelectList CategoryList { get; set; }
        public EditModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        [BindProperty]
        public List<IFormFile> Images { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id) ?? new();
            CategoryList = new SelectList(_db.Categories, "Id", "Name");
            if (Product == null) return NotFound();
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var dbProduct = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == Product.Id);
            if (dbProduct == null) return NotFound();

            dbProduct.Name = Product.Name;
            dbProduct.CategoryId = Product.CategoryId;
            dbProduct.Description = Product.Description;
            dbProduct.Price = Product.Price;

            if (Images != null && Images.Count > 0)
            {
                var uploadsDir = Path.Combine(_env.WebRootPath, "productimg");
                foreach (var img in Images)
                {
                    if (img != null && img.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(img.FileName)}";
                        var filePath = Path.Combine(uploadsDir, fileName);
                        using var stream = new FileStream(filePath, FileMode.Create);
                        await img.CopyToAsync(stream);
                        _db.ProductImages.Add(new ProductImage
                        {
                            ProductId = dbProduct.Id,
                            ImageUrl = $"/productimg/{fileName}"
                        });
                    }
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}