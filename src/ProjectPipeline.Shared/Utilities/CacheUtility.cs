using System.Collections.Concurrent;

namespace ProjectPipeline.Shared.Utilities;

/// <summary>
/// Simple in-memory cache utility
/// </summary>
public static class CacheUtility
{
    private static readonly ConcurrentDictionary<string, CacheItem> _cache = new();

    /// <summary>
    /// Gets an item from cache
    /// </summary>
    /// <typeparam name="T">Type of cached item</typeparam>
    /// <param name="key">Cache key</param>
    /// <returns>Cached item or default</returns>
    public static T? Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out var item) && !item.IsExpired)
        {
            return (T)item.Value;
        }

        // Remove expired item
        if (item?.IsExpired == true)
        {
            _cache.TryRemove(key, out _);
        }

        return default;
    }

    /// <summary>
    /// Sets an item in cache
    /// </summary>
    /// <typeparam name="T">Type of item to cache</typeparam>
    /// <param name="key">Cache key</param>
    /// <param name="value">Value to cache</param>
    /// <param name="expiration">Expiration time</param>
    public static void Set<T>(string key, T value, TimeSpan expiration)
    {
        var item = new CacheItem(value!, DateTime.UtcNow.Add(expiration));
        _cache.AddOrUpdate(key, item, (k, v) => item);
    }

    /// <summary>
    /// Removes an item from cache
    /// </summary>
    /// <param name="key">Cache key</param>
    public static void Remove(string key)
    {
        _cache.TryRemove(key, out _);
    }

    /// <summary>
    /// Clears all cache items
    /// </summary>
    public static void Clear()
    {
        _cache.Clear();
    }

    /// <summary>
    /// Gets or sets a cached item
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    /// <param name="key">Cache key</param>
    /// <param name="factory">Factory function to create item if not cached</param>
    /// <param name="expiration">Expiration time</param>
    /// <returns>Cached or newly created item</returns>
    public static T GetOrSet<T>(string key, Func<T> factory, TimeSpan expiration)
    {
        var cached = Get<T>(key);
        if (cached != null)
            return cached;

        var value = factory();
        Set(key, value, expiration);
        return value;
    }

    private class CacheItem
    {
        public object Value { get; }
        public DateTime ExpiresAt { get; }
        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        public CacheItem(object value, DateTime expiresAt)
        {
            Value = value;
            ExpiresAt = expiresAt;
        }
    }
}
