using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using BookStoreProject.Models;
using BookStoreProject.Services;
using BookStoreProject.Commons;
using Microsoft.AspNetCore.Authorization;

namespace BookStoreProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private IOrdersService _ordersService; 

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        

    }
}
