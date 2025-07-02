using AashaGifts.Web.Data;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AashaGifts.Web.Pages
{
    public class AdminModel : PageModel
    {
        private readonly AppDbContext _db;

        public AdminModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Order> Orders { get; set; } = new List<Order>();

        public void OnGet()
        {
            Orders = _db.Orders.OrderByDescending(o => o.OrderDate).ToList();
        }

        public async Task<IActionResult> OnPostMarkCompletedAsync(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                order.Status = "Completed";
                await _db.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}
