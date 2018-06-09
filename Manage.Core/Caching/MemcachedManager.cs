using Manage.Core.Utility;
using Memcached.ClientLibrary;
using System;

namespace Manage.Core.Caching
{
    public class MemcachedManager : ICacheManager
    {
        private static MemcachedClient CreateServer()
        {
            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(ConfigUtil.GetValue("MemcachedServers").Split(','));
            pool.Failover = true;
            pool.Initialize();

            //客户端实例
            MemcachedClient mc = new MemcachedClient
            {
                EnableCompression = false
            };

            return mc;
        }

        public void Set(string key, object value)
        {
            MemcachedClient mcmain = CreateServer();
            mcmain.Set(key, value);
        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {
            MemcachedClient mcmain = CreateServer();
            mcmain.Set(key, value, DateTime.Now.AddMinutes(30));
        }

        public void Remove(string key)
        {
            MemcachedClient mcmain = CreateServer();
            mcmain.Delete(key);
        }

        public void Clear()
        {
            MemcachedClient mcmain = CreateServer();
            mcmain.FlushAll();
        }

        public bool Contains(string key)
        {
            MemcachedClient mc = CreateServer();
            return mc.KeyExists(key);
        }

        public T Get<T>(string key)
        {
            MemcachedClient mc = CreateServer();
            return (T)mc.Get(key);
        }
    }
}
