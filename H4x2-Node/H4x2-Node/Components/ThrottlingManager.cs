using LazyCache;
using Microsoft.Extensions.Caching.Memory;
namespace H4x2_Node.Controllers
{
    public class ThrottlingManager
    {
        public int Allow { get; set; }
        public int Lapse { get; set; }
        public int MaxPenalty { get; set; }
        private readonly IAppCache _cache;
    

        public ThrottlingManager()
        {
            Allow = 3;
            Lapse = 60; // 1 Minute unit
            MaxPenalty = 60 * 60; // Max 1 hour throttle
            _cache = new CachingService();

        }

        public async Task<int> Throttle(string id)
        {
            var entry = await _cache.GetOrAddAsync<CacheEntry>(id, () => Task.Run(() => new CacheEntry()), BuildPolicy(TimeSpan.FromSeconds(Lapse)));

            if (entry is not null)
            {
                if (entry.Times > 3 && DateTime.Compare(entry.Start.AddSeconds(entry.SlidingExpiration), DateTime.UtcNow) > 0)
                    return (int) Math.Floor((entry.Start.AddSeconds(entry.SlidingExpiration) - DateTime.UtcNow).TotalSeconds);
                    
                var penalty = (int)Math.Min(Math.Floor(Math.Pow(2, entry.Times) * Lapse), MaxPenalty);
                Interlocked.Increment(ref entry.Times);
                entry.Start = DateTime.UtcNow;
                entry.SlidingExpiration =  penalty;
                if (penalty < MaxPenalty)
                    _cache.Add(id, entry, BuildPolicy(TimeSpan.FromSeconds(penalty)));   // Call Add() with the generated value you want to update into the cache and it will force the item to be replaced         

                bool isThrottled = entry.Times > Allow;

                return isThrottled ? penalty : 0;
            }
            else
                return MaxPenalty;
        }
        
        public void Remove(string id) => _cache.Remove(id);


        private MemoryCacheEntryOptions BuildPolicy(TimeSpan timespam) => (new MemoryCacheEntryOptions())
            .SetPriority(CacheItemPriority.NeverRemove) //When the application server is running short of memory, the .NET Core runtime will initiate the clean-up of In-Memory cache items other than the ones set with NeverRemove priority
            .SetSlidingExpiration(timespam)
            .RegisterPostEvictionCallback(PostEvictionCallback);

        public void PostEvictionCallback(object key, object value, EvictionReason reason, object state)
        {
            if (reason == EvictionReason.Capacity)
                Console.WriteLine("Evicted due to {0}", reason); // change to log fot troubleshooting

        }
        public class CacheEntry
        {
            public int Times = 0;
            public DateTime Start = DateTime.UtcNow;
            public int SlidingExpiration = 60;

        }
    }
}