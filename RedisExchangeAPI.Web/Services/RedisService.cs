using StackExchange.Redis;

namespace RedisExchangeAPI.Web.Services
{
    public class RedisService
    {
        private readonly string _redisHost;

        private readonly string _redisPort;


        private ConnectionMultiplexer _redis; // Redis server ile haberleşeceğim class.


        public IDatabase db { get; set; }



        public RedisService(IConfiguration configuration) 
        {
            _redisHost = configuration["Redis:Host"];

            _redisPort = configuration["Redis:Port"];

        }



        public void Connect()
        {
            var configString = $"{_redisHost}:{_redisPort}";

            _redis = ConnectionMultiplexer.Connect(configString); // Redis ile haberleşmeyi sağlıyoruz.
        }



        public IDatabase GetDb(int db)
        {
            return _redis.GetDatabase(db); // Artık db üzerinden birçok işimizi halledebileceğiz. Redis Metotlarımıza bağlanabileceğiz.
        }
    }
}
