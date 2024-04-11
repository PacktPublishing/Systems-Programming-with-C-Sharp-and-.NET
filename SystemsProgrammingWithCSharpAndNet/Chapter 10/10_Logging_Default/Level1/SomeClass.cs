using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace _10_Logging_Default.Level1
{
    internal class SomeClass(ILogger logger)
    {
        public void DoIt()
        {
            logger.LogInformation("Information in SomeClass");
            logger.LogError("Error in SomeClass");
        }
    }
}
