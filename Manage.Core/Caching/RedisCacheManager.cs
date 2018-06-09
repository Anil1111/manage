using Manage.Core.Utility;
using StackExchange.Redis;
using System;
using System.Text;

namespace Manage.Core.Caching
{
    public class RedisCacheManager : ICacheManager
    {
        private readonly string redisConnectionString;
        private volatile ConnectionMultiplexer redisConnection;
        private readonly object redisConnectionlocker = new object();

        public RedisCacheManager()
        {
            this.redisConnectionString = ConfigUtil.GetValue("RedisConnectionString");
            this.redisConnection = GetRedisConnection();
        }

        private ConnectionMultiplexer GetRedisConnection()
        {
            if (this.redisConnection != null && this.redisConnection.IsConnected)
            {
                return redisConnection;
            }

            lock (redisConnectionlocker)
            {
                if (this.redisConnection != null)
                {
                    redisConnection.Dispose();
                }

                this.redisConnection = ConnectionMultiplexer.Connect(redisConnectionString);
            }

            return this.redisConnection;
        }

        public void Clear()
        {
            foreach (var endPoint in this.redisConnection.GetEndPoints())
            {
                var server = this.GetRedisConnection().GetServer(endPoint);
                foreach (var key in server.Keys())
                {
                    redisConnection.GetDatabase().KeyDelete(key);
                }
            }
        }

        public bool Contains(string key)
        {
            return redisConnection.GetDatabase().KeyExists(key);
        }

        public T Get<T>(string key)
        {
            var value = redisConnection.GetDatabase().StringGet(key);
            if (value.HasValue)
            {
                return Deserialize<T>(value);
            }
            else
            {
                return default(T);
            }
        }

        public void Remove(string key)
        {
            redisConnection.GetDatabase().KeyDelete(key);
        }

        public void Set(string key, object value)
        {
            if (value != null)
            {
                redisConnection.GetDatabase().StringSet(key, Serialize(value));
            }
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            if (value != null)
            {
                redisConnection.GetDatabase().StringSet(key, Serialize(value), cacheTime);
            }
        }

        private static byte[] Serialize(object o)
        {
            var jsonString = JsonUtil.SerializerObject(o);
            return Encoding.UTF8.GetBytes(jsonString);
        }

        private static T Deserialize<T>(byte[] value)
        {
            if (value == null)
            {
                return default(T);
            }
            var jsonString = Encoding.UTF8.GetString(value);
            return JsonUtil.DeserializeJsonToObject<T>(jsonString);
        }
    }
}
