using System;
using System.Runtime.Caching;

namespace PaypalTestApp.Helpers
{
    public sealed class CacheManager
    {
        private const int DEFAULT_PAYPAL_EXPIRATION_SECONDS = 32398;
        private readonly ObjectCache cache;
        private readonly CacheItemPolicy defaultCacheItemPolicy;

        private static readonly object syncRoot = new object();
        private static CacheManager instance;

        private CacheManager()
        {
            this.cache = MemoryCache.Default;
            this.defaultCacheItemPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(DEFAULT_PAYPAL_EXPIRATION_SECONDS)
            };
        }

        public static CacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new CacheManager();
                        }
                    }
                }

                return instance;
            }
        }

        public T Get<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return default(T);
            }

            try
            {
                if (this.cache.Contains(key))
                {
                    return (T) this.cache.Get(key);
                }

                return default(T);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public bool Add<T>(string key, T data, CacheItemPolicy itemPolicy = null)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    return false;
                }

                if (itemPolicy == null)
                {
                    itemPolicy = this.defaultCacheItemPolicy;
                }

                lock (syncRoot)
                {
                    return this.cache.Add(key, data, itemPolicy);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}