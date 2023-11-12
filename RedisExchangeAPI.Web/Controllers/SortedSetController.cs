using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class SortedSetController : Controller
    {

        private readonly RedisService _redisService;

        private readonly IDatabase db; // Redis database bağlanıyoruz.

        private string sortSet = "sortSet_Names";



        public SortedSetController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }



        public IActionResult Index()
        {
            // HashSet Nesne örneği alıyoruz.
            HashSet<string> list = new HashSet<string>();

            if(db.KeyExists(sortSet))
            {
                // veritabanı sırasına göre değerimizi alacağız. Hem value hem score.

                //  db.SortedSetScan(sortSet).ToList().ForEach(x =>
                //   {
                //       list.Add(x.ToString()); 
                //  });

                //Value'nin kesisi geliyor Büyükten küçüğe doğru.
                db.SortedSetRangeByRank(sortSet, order: Order.Descending).ToList().ForEach(x =>
                {
                    list.Add(x.ToString());
                });

                // N O T
                // db.SortedSetRangeByRank(sortSet,0,5, order: Order.Descending) --> BU KISIMDA İSE İNDEX DEĞERLERİNİ VERİP O ARALIKTAKİ DEĞERLERİ ALABİLİRİZ.


            }
            return View(list); // model olarak dönüyoruz.
        }




        [HttpPost]
        // Sorted Set olduğu için bir de score değeri alır.
        public IActionResult Add(string name, int score) 
        {

            db.SortedSetAdd(sortSet, name, score);
            // key   -    üye   -   score
            // Hash Set'de name kısmı aynı olamaz ama score kısmı olabilir.

            db.KeyExpire(sortSet, DateTime.Now.AddMinutes(1));
            // Cache'miz 1 dk sonra silinecek.

            return RedirectToAction("Index");

        }

        // Cechelerimizi silen metot
        public IActionResult DeleteItem(string name) 
        {
            db.SortedSetRemove(sortSet, name);//Silme işlemi uygulanıyor.

            return RedirectToAction("Index");
        }
    }
}
