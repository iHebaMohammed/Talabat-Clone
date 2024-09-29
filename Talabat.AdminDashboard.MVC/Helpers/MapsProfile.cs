using AutoMapper;
using Talabat.AdminDashboard.MVC.ViewModels;
using Talabat.Core.Entities;

namespace Talabat.AdminDashboard.MVC.Helpers
{
    public class MapsProfile:Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
