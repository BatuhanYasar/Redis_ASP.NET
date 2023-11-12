using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class ListTypeController : Controller
    {

        private readonly RedisService _redisService;

        private readonly IDatabase db; // Database'ye karşılık gelir.


        private string listKey = "names"; // Liste key'imiz


        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;

            db = _redisService.GetDb(1);
        }


        public IActionResult Index()
        {
            List<string> namesList = new List<string>();
            
            if(db.KeyExists(listKey)) // listKey varsa listeyi sıralayacak.
            {
                db.ListRange(listKey).ToList().ForEach(x=>
                {
                    namesList.Add(x.ToString());
                }
                    );
            }

            return View(namesList);
        }


        // Veri ekleme işlemi yaptığımız metot.
        [HttpPost]
        public IActionResult Add(string name) 
        {
            db.ListRightPush(listKey,name); // Verileri cachelerken sona ekliyor.

            return RedirectToAction("Index");
        }



      // vERİ SİLME mEYODU
        public IActionResult DeleteItem(string name) 
        {
          db.ListRemoveAsync(listKey,name).Wait();
            // Geri dönüş ile ilgilenmiyoruz o yüzden Wait() kullandık.


            return RedirectToAction("Index");
        }


        // iLK CACHE'Yİ SİLME METODU.
        public IActionResult DeleteFirstItem()
        {
            db.ListLeftPop(listKey); // İlk sıradaki cache'yi siler.

            return RedirectToAction("Index");
        }
    }
}
