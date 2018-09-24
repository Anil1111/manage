using System;

namespace Manage.Core.Caching
{
    /// <summary>
    /// 缓存
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        void Set(string key, object value);

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="cacheTime">过期时间</param>
        void Set(string key, object value, TimeSpan cacheTime);

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">键</param>
        void Remove(string key);

        /// <summary>
        /// 清除缓存
        /// </summary>
        void Clear();
    }
}
