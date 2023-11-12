using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SetTypeController : Controller
    {

        private readonly RedisService _redisService;

        private readonly IDatabase db; // redis database'sine bağlanırız.

        private string listKey = "hash_Names";




        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
            // Redis Desktop Manager'de bulunan değerlere kaydediyor.
        }


        public IActionResult Index()
        {
            HashSet<string> nameList = new HashSet<string>();

            if (db.KeyExists(listKey))
            {
                db.SetMembers(listKey).ToList().ForEach(x =>
                {

                nameList.Add(x.ToString());

                });
            }

            return View(nameList);
        }


        [HttpPost]
        public IActionResult Add(string name) 
        {

            if(!db.KeyExists(listKey)) // Eğer cache varsa süre eklemeyecek.
            {
                // Cache'ye var olma süresi verdik.
                db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));
            }

          db.SetAdd(listKey, name);

          return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteItem(string name) 
        {
           await db.SetRemoveAsync(listKey, name);

            return RedirectToAction("Index");
        }
    }
}
