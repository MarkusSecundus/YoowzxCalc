using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkusSecundus.YoowzxCalc.Cmd
{
    public class FailException : Exception
    {
        public FailException(string message=null) : base(message??"failed") { }
    }
}
