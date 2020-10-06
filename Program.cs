using System;
using System.Linq;

namespace RandomFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine( "RandomFiles" );
            Console.WriteLine( "----------------" );

            if (args.Contains("--help") || args.Contains("-h")) {
                Console.WriteLine( Help.MainHelp() );
                return;
            }


        }
    }
}
