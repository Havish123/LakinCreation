using AashaGifts.Web.Data;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AashaGifts.Web.Pages.Products
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CreateModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        [BindProperty]
        public List<IFormFile> Images { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            if (Images == null || Images.Count == 0)
            {
                ModelState.AddModelError("Images", "Please upload at least one image.");
                return Page();
            }

            _db.Products.Add(Product);
            await _db.SaveChangesAsync();

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
                        ProductId = Product.Id,
                        ImageUrl = $"/productimg/{fileName}"
                    });
                }
            }
            await _db.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}