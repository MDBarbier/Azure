using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConfigurationMode
{
    class Program
    {
        static void Main(string[] args)
        {

#if DEBUG
            /* Debug mode logic - switch to Release mode in VS to get Intellisense! */

            Console.WriteLine("Debug mode");

#else
            /* Release mode logic - switch to Release mode in VS to get Intellisense! */

            Console.WriteLine("Debug mode");
#endif


        }
    }
}
