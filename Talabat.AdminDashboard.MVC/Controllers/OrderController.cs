using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.IRepositories;
using Talabat.Core.Specifications;

namespace Talabat.AdminDashboard.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new OrderSpecification();
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(spec);
            return View(orders);
        }
        public async Task<IActionResult> GetUserOrders(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecificationAsync(spec);

            return View("Index", orders);
        }
    }
}
