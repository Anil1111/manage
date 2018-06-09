using System;

namespace Manage.Core.Caching
{
    public interface ICacheManager
    {
        void Set(string key, object value);

        void Set(string key, object value, TimeSpan cacheTime);

        bool Contains(string key);

        T Get<T>(string key);

        void Remove(string key);

        void Clear();
    }
}
