using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.Util
{
    /// <summary>
    /// Container for types needed to describe a function
    /// </summary>
    public struct FunctionParametersInfo
    {
        /// <summary>
        /// Ordered list of argument types
        /// </summary>
        public Type[] Args { get; init; }
        /// <summary>
        /// Type of the return value
        /// </summary>
        public Type Ret { get; init; }
    }
}
