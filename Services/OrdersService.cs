using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreProject.Models;
using BookStoreProject.Infrastructure;
using BookStoreProject.Repositorys;


namespace BookStoreProject.Services
{
    public interface IOrdersService : ICommonService<Orders>
    {

    }
    public class OrdersService:CommonService<Orders,IOrdersRepository>,IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrdersService(IUnitOfWork unitOfWork, IOrdersRepository ordersRepository) : base(unitOfWork, ordersRepository)
        {
            _ordersRepository = ordersRepository;
            _unitOfWork = unitOfWork;
        }
    }
}
