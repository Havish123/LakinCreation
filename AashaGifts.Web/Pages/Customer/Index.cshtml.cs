using AashaGifts.Web.Data;
using AashaGifts.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace AashaGifts.Web.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _db;

        public IndexModel(AppDbContext db)
        {
            _db = db;
        }

        public List<Order> Orders { get; set; } = new();

        public void OnGet()
        {
            var userId = User.Identity?.IsAuthenticated == true
                ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                : null;
            Orders = _db.Orders.Where(o => o.UserId == userId).OrderByDescending(o => o.OrderDate).ToList();
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
