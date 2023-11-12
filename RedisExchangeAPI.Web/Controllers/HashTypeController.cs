using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    //  BaseController'dan miras alıyor.
    public class HashTypeController : BaseController
    {
        // keyimize sozluk verdik.
        public string hashKey { get; set; } = "sozluk";

        public HashTypeController(RedisService redisService) : base(redisService)
        {
        }

        public IActionResult Index()
        {
            Dictionary<string,string> list = new Dictionary<string,string>();

            if(db.KeyExists(hashKey))
            {
                db.HashGetAll(hashKey).ToList().ForEach(x =>
                {

                });
            }

            return View();
        }



        [HttpPost]
        public IActionResult Add(string name, string value)
        {
            db.HashSet(hashKey, name, value);  

            return RedirectToAction("Index");
        }


        public IActionResult DeleteItem(string name) 
        {
           db.HashDelete(hashKey, name);
            return RedirectToAction("Index");
        }
    }
}
