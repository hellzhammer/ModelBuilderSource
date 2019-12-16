using System;
using System.IO;
using System.Linq;

using Output = System.Console;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestApp.StartupLogic
{
    public class StartupMLBuilder
    {
        public StartupMLBuilder()
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "/output"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/output");
            }
        }
    }
}
