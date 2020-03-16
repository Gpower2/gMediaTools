using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services
{
    public static class ServiceFactory
    {
        private static readonly ConcurrentDictionary<Type, object> _servicesCache = new ConcurrentDictionary<Type, object>();

        private static object _lockAnchor = new object();

        public static T GetService<T>() where T : new()
        {
            // Check if it is already contained in cache
            if (_servicesCache.ContainsKey(typeof(T)))
            {
                return (T)_servicesCache[typeof(T)];
            }
            
            // Lock the operation
            lock (_lockAnchor)
            {
                // Create the new service
                T service = new T();
                
                // Check for successful service creation
                if (service == null)
                {
                    throw new Exception($"Could not create {typeof(T)} in services factory!");
                }
                
                // Add the new service to the cache and check if it was successful
                if (!_servicesCache.TryAdd(typeof(T), service))
                {
                    throw new Exception($"Could not add {typeof(T)} in services cache!");
                }

                // Return the newly created service
                return service;
            }
        }
    }
}
