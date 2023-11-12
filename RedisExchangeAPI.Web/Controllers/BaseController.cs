using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly RedisService _redisService;

        protected readonly IDatabase db; // Database'ye karşılık gelir.


        // Uzaktan bağlantı kuracağımız için ( birçok controller buradan yararlanabilir. protected yaptık. )


        public BaseController(RedisService redisService)
        {
            _redisService = redisService;

            db = _redisService.GetDb(1);
        }
    }
}
