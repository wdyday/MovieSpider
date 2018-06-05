using System;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSpider.Core.Utils
{
    public class CacheUtil
    {
        private static List<string> _allUseCacheKey = new List<string>();
        private static ObjectCache _cache = MemoryCache.Default;

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">Key 唯一</param>
        /// <param name="value">值</param>
        /// <param name="cacheOffset">超时时间</param>
        public static void Add(string key, object value, DateTimeOffset cacheOffset)
        {
            if (_allUseCacheKey.Contains(key))
            {
                Remove(key);
            }
            _allUseCacheKey.Add(key);
            _cache.Add(key, value, cacheOffset);
        }

        /// <summary>
        /// 添加永久缓存
        /// </summary>
        /// <param name="key">Key 唯一</param>
        /// <param name="value">值</param>
        public static void Add(string key, object value)
        {
            Add(key, value, DateTimeOffset.Now.AddYears(100));
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">Key</param>
        public static void Remove(string key)
        {
            if (_allUseCacheKey.Contains(key))
            {
                _allUseCacheKey.Remove(key);
            }
            _cache.Remove(key);
        }

        /// <summary>
        /// 读取缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static object Get(string key)
        {
            if (_allUseCacheKey.Contains(key))
                return _cache[key];
            return null;
        }

        public static TOut Get<TOut>(string key)
        {
            var cache = Get(key);

            return (TOut)cache;
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Clear()
        {
            foreach (string value in _allUseCacheKey)
            {
                _cache.Remove(value);
            }
            _allUseCacheKey.Clear();
        }
    }
}
