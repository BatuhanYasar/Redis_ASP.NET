using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {

        private readonly RedisService _redisService;

        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
        }


        public IActionResult Index()
        {
            var db = _redisService.GetDb(0);
            // 0.db yi alıyoruz.

            // Üç adet data kaydediyoruz
            db.StringSet("name", "Batuhan YAŞAR");
            db.StringSet("ziyaretçi", 100);
            db.StringSet("kişi", 100);

            return View();
        }

        public IActionResult Show() 
        {
            // StringGet ile value'imizi alıyoruz.
            var value = db.StringGet("name");

            // Ben iki index aralığındaki karakterimi veriyorum. O da verilen değer aralığında kelimemizi getiriyor.
            var value2 = db.StringGetRange("name", 0, 3);

            // Key'imizin uzunluğunu dönecek integer olarak.
            var valueLength = db.StringLength("name");

            // Burada ziyaretçi adlı value'mizi arttıracağız.(10 yazdık biz.)
            db.StringIncrement("ziyaretçi", 10);


            // Burada ziyaretçi adlı value'mizi azaltacağız.(1 yazdık biz.)
            // Değişkene atıyorsak async metodu Result ile alabiliriz.
            var count = db.StringDecrementAsync("kişi", 1).Result;


            // Eğer value null değilse veriyi viewbag View ekranında yansıtacağız.
            if (value.HasValue) 
            {
                ViewBag.value = value.ToString();
                ViewBag.count = count;
                ViewBag.valueLength = valueLength;
            }


          return View();
        }
    }
}
