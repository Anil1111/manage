using System;

namespace Manage.Core.Caching
{
    public class NullCacheManager : ICacheManager
    {
        public void Clear()
        {

        }

        public bool Contains(string key)
        {
            return false;
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public void Remove(string key)
        {

        }

        public void Set(string key, object value)
        {

        }

        public void Set(string key, object value, TimeSpan cacheTime)
        {

        }
    }
}
