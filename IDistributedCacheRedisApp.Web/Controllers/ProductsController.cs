using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;


        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        /*         
         *       ---  Burada string serilize yaptık. ---
         *         
                public async Task<IActionResult> Index()
                {
                    DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

                    cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);

                    // 1. Burada basit cache işlemi için bir keye değer atadık.
                    // _distributedCache.SetString("name","Batuhan",cacheEntryOptions);

                    // await _distributedCache.SetStringAsync("surname", "YASAR");



                    Product product = new Product { Id = 1, Name = "Kalem", Price = 200 }; 
                    // a.1. JSON Serilize işlemi gerçekletiriyoruz.
                    string jsonProduct =  JsonConvert.SerializeObject(product);

                    await _distributedCache.SetStringAsync("product:1", jsonProduct,cacheEntryOptions);


                    return View();
                }

                public IActionResult Show()
                {    // 2. Burada cache işlemin alıyoruz ve Viewbag ile yansıtıyor.
                     // string name = _distributedCache.GetString("name");


                    // ViewBag.name = name;

                    string jsonProduct = _distributedCache.GetString("product:1");
                    // a.2. JSON Deserilize işlemi gerçekletiriyoruz.
                    Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

                    ViewBag.product = p;

                    return View();
                }
        */

        //               --- Byte ile Serilize Yapıyoruz ---

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(10);


            Product product = new Product { Id = 1, Name = "Kalem", Price = 200 };
            // a.1. JSON Binary Serilize işlemi gerçekletiriyoruz.
            string jsonProduct = JsonConvert.SerializeObject(product);

            Byte[] byteProduct = Encoding.UTF8.GetBytes(jsonProduct);

            _distributedCache.Set("product:1", byteProduct);


            return View();
        }

        public IActionResult Show()
        {
            Byte[] byteProduct = _distributedCache.Get("product:1");

            string jsonProduct = Encoding.UTF8.GetString(byteProduct);

            // a.2. JSON Deserilize işlemi gerçekletiriyoruz.
            Product p = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.product = p;

            return View();
        }

        public IActionResult Remove()
        {
            // 3. Burada basit cache işlemini siliyor.
            _distributedCache.Remove("name");

            return View();

        }


        public IActionResult ImageUrl()
        {
            byte[] resimByte = _distributedCache.Get("resim");

            return File(resimByte, "image/png");
        }


        // Image Cache'liyoruz.
        public IActionResult ImageCache() 
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/araba.png"); // Görselimizin adres yolu.

            byte[] imageByte =System.IO.File.ReadAllBytes(path); //byte[] kullanıyoruz.

            _distributedCache.Set("resim", imageByte); // Cache etmek için set ediyoruz.

           return View();
        }



    }
}
