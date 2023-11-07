using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IMemoryCache _memoryCache; // IMemoryCache ekledik.



        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }




        public IActionResult Index()
        {
            // Bir key değerinin memoryde olup olmadığını öğrenmek için.


            //1.Yol
            if(String.IsNullOrEmpty(_memoryCache.Get<string>("zaman")))
            {
                // Zamanı cachelyoruz.
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
                // zaman key'ine sahip bir cache kaydedilecek.  
            }

            //2.Yol
            if (!_memoryCache.TryGetValue("zaman", out string zamanCache))
            {
                _memoryCache.Set<string>("zaman", DateTime.Now.ToString());
            }
                // Olursa true dönecekse ve zaman key'inin değeri zamanCache de gösterlecek. 

            return View();
        }






        public IActionResult Show()
        {
            // Önce almaya çalışır eğer alamazsa gidip memoryde oluşturur.
            _memoryCache.GetOrCreate<string>("zaman", entry =>
            {
                // entry ise önceliklerini yazmak için eklenir.
                return DateTime.Now.ToString();
            });


            // Datayı memory'den siler.
            //_memoryCache.Remove("zaman");

           ViewBag.zaman = _memoryCache.Get<string>("zaman");
            // zaman key'ine sahip br cache okunacak.

            return View();
        }
    }
}
