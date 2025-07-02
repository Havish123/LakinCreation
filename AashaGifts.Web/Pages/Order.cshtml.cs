using AashaGifts.Web.Data;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AashaGifts.Web.Pages
{
    public class OrderModel : PageModel
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public OrderModel(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        [BindProperty]
        public OrderInputModel OrderInput { get; set; } = new();

        [BindProperty]
        public IFormFile? Photo { get; set; }

        public Product? Product { get; set; }

        public class OrderInputModel
        {
            [Required]
            [Display(Name = "Your Name")]
            public string CustomerName { get; set; } = "";

            [Required]
            [EmailAddress]
            [Display(Name = "Email Address")]
            public string Email { get; set; } = "";
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _db.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
            if (Product == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Product = await _db.Products.FindAsync(id);
            if (Product == null)
                return NotFound();

            if (!ModelState.IsValid)
                return Page();

            if (Photo == null || Photo.Length == 0)
            {
                ModelState.AddModelError("Photo", "Please upload an image.");
                return Page();
            }

            // Save photo
            var uploadsDir = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileExt = Path.GetExtension(Photo.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExt}";
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Photo.CopyToAsync(stream);
            }

            // Save order
            var order = new Order
            {
                CustomerName = OrderInput.CustomerName,
                Email = OrderInput.Email,
                ProductId = Product.Id,
                ProductName = Product.Name,
                UploadedPhotoPath = $"/uploads/{fileName}",
                Status = "Pending",
                UserId = User.Identity?.IsAuthenticated == true ? User.FindFirst("sub")?.Value : null
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            return RedirectToPage("Thanks");
        }
    }
}