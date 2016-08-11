using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWAProtocolUtils
{ 
    static public class Log
    {
        public static void WriteLine(string str)
        {
            Console.WriteLine(str);
        }

        public static void Write(string str)
        {
            Console.Write(str);
        }

        public static void WriteLine(int i)
        {
            Console.WriteLine(i);
        }

        public static void Write(int i)
        {
            Console.Write(i);
        }
    }
}
