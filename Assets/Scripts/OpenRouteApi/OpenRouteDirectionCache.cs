using System;
using System.Collections.Generic;
using System.Linq;


public class OpenRouteDirectionCache 
{
    private class DirectionCacheEntry
    {
        public Uri address;
        public Response requestCollections;
    }

    private Queue<DirectionCacheEntry> cache;

    private int capacity;

    public OpenRouteDirectionCache(int capacity)
    {
        this.cache = new Queue<DirectionCacheEntry>();
        this.capacity = capacity;
    }

    public Response Get(Uri address)
    {
        Response requestCollections = null;

        lock (cache)
        {
            if (this.cache.Count > 0)
            {
                var cacheEntry = this.cache.FirstOrDefault(entry => entry.address.Equals(address));
                if (cacheEntry != null)
                {
                    requestCollections = cacheEntry.requestCollections;
                }
            }
        }

        return requestCollections;
    }

    public void Clear()
    {
        lock (cache)
        {
            cache.Clear();
        }
    }

    public void Add(Uri address, Response requestCollections)
    {
        lock (cache)
        {
        DirectionCacheEntry cacheEntry = null;

            if (cache.Count > 0)
            {
                cacheEntry = this.cache.FirstOrDefault(entry => entry.address.Equals(address));
            }

            if (cacheEntry == null)
            {
                var entry = new DirectionCacheEntry();
                entry.address = address;
                entry.requestCollections = requestCollections;

                cache.Enqueue(entry);

                if (cache.Count > capacity)
                {
                    cache.Dequeue();
                }
            }
            else
            {
                cacheEntry.requestCollections = requestCollections;
            }
        }
    }

}

