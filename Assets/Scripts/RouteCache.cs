using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TableTop
{
    public class VertTriang
    {
        public Vector3[] vertices;
        public int[] triangles;

        public VertTriang(Vector3[] vert,int[] tri)
        {
            this.vertices = vert;
            this.triangles = tri;
        }
    }

    public class RouteCache
    {
        
        private class RouteCacheEntry
        {
            public string name;
            public VertTriang requestCollections;
        }

        private Queue<RouteCacheEntry> cache;

        private int capacity;

        public RouteCache(int capacity)
        {
            this.cache = new Queue<RouteCacheEntry>();
            this.capacity = capacity;
        }

        public VertTriang Get(string name)
        {
            VertTriang requestCollections = null;

            lock (cache)
            {
                if (this.cache.Count > 0)
                {
                    var cacheEntry = this.cache.FirstOrDefault(entry => entry.name.Equals(name));
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

        public void Add(string name, VertTriang requestCollections)
        {
            lock (cache)
            {
                RouteCacheEntry cacheEntry = null;

                if (cache.Count > 0)
                {
                    cacheEntry = this.cache.FirstOrDefault(entry => entry.name.Equals(name));
                }

                if (cacheEntry == null)
                {
                    var entry = new RouteCacheEntry();
                    entry.name = name;
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
}

