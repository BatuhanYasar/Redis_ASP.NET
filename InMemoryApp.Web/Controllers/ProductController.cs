using InMemoryApp.Web.Models;
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


            //2.Yol

                // Options kısmında cache için süre ayarlıyoruz.
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

            // Absolute Expiration
            // Süreyi 20 sn olarak ayarladık.
            //options.AbsoluteExpiration= DateTime.Now.AddSeconds(20);

            // Ömrü 5 kere artar --> 1 minute = 60 seconds

            options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            // Sliding Expiration
            options.SlidingExpiration = TimeSpan.FromSeconds(10);

            // Cache Priority
            options.Priority = CacheItemPriority.High;
            // Low --> Önce bunu sil.
            // High --> Silme.
            // Normal --> Low yoksa Bunu siler.


            // RegisterPostEvictionCallback metodu bizim cachemizin neden silindiğini dönmemize olanak sağlıyor.
            options.RegisterPostEvictionCallback((key, value, state, reason) =>
            {
                _memoryCache.Set("callback",$"{key}-->{value} => sebep : {reason}");
            });



                _memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);

                // Olursa true dönecekse ve zaman key'inin değeri zamanCache de gösterlecek. 


            // Bir adet ürün oluşturduk.
            Product product = new Product {Id=1,Name="Kalem",Price =200};

            _memoryCache.Set<Product>("product:1", product); // Oluşturduğumuz ürünü cacheledik.

            _memoryCache.Set<double>("money", 119.99); // Burada da örnek olsun dye biir double nasıl cachelenir onu yazdık.


            return View();

        }






        public IActionResult Show()
        {
            _memoryCache.TryGetValue("zaman", out string zamanCache);
            // zaman adlı bir veri varsa zamanCache'ye atayacak.

            _memoryCache.TryGetValue("callback", out string callback);

            // Datayı memory'den siler.
            //_memoryCache.Remove("zaman");

            ViewBag.zaman = zamanCache;
            // zaman key'ine sahip bir cache okunacak.

            ViewBag.callback = callback;
            // Silinen cachenin sebebini view ekranında yansıtacağız.



            //ürünümüzün cachesini alıyoruz.
            ViewBag.product = _memoryCache.Get<Product>("product:1");

            return View();
        }
    }
}
