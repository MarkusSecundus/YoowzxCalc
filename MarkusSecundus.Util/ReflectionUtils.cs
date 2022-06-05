using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Assembly> IterateAllAssemblies()
        {
            var alreadyVisited = new HashSet<Assembly>();

            var stack = new List<Assembly>();
            stack.AddRange(AppDomain.CurrentDomain.GetAssemblies());

            while (stack.Count > 0)
            {
                var a = stack.Pop();
                if (alreadyVisited.Contains(a)) continue;
                alreadyVisited.Add(a);

                foreach (var referencedAssembly in a.GetReferencedAssemblies())
                {
                    stack.Push(Assembly.Load(referencedAssembly));
                }
                yield return a;
            }
        }
    }
}
