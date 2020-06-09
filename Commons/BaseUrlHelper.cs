using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BookStoreProject.Commons
{
    public interface IBaseUrlHelper
    {
        string GetBaseUrl();
    }
    public class BaseUrlHelper:IBaseUrlHelper
    {
        private readonly IHttpContextAccessor _context;

        public BaseUrlHelper(IHttpContextAccessor context)
        {
            _context = context;
        }
        public string GetBaseUrl()
        {
            var request = _context.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            return $"{request.Scheme}://{host}{pathBase}";
        }
    }
}
