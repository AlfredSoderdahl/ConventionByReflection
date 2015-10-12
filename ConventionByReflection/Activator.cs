using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConventionByReflection {
    public class Activator {
        public static IEnumerable<T> CreateInstances<T>(Assembly rootAssembly, Func<Type, object> activator = null) {
            var assemblies =
                rootAssembly.GetReferencedAssemblies()
                            .Select(Assembly.Load)
                            .Union(new[] {rootAssembly})
                            .Except(new[] {typeof (Activator).Assembly});
            var implementations =
                assemblies.SelectMany(
                    a => a.GetTypes()
                          .Where(t => t.IsClass &&
                                      !t.IsAbstract &&
                                      typeof (T).IsAssignableFrom(t)));

            var instances = implementations.Select(activator ?? System.Activator.CreateInstance)
                                           .OfType<T>();
            return instances;
        }
    }
}