using System;
using System.IO;

namespace pack_html
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // verify passed file is legit
            if (args[0] == null)
            {
                Console.WriteLine("No file passed");
                Exit(1);
            }
            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File " + args[0] + "does not exist, try again");
                Exit(1);
            }


            var packer = new Packer(args[0]);
            packer.Pack();


            Exit();
        }

        private static void Exit(int errCode = 0)
        {
            Console.WriteLine(Environment.NewLine + "Press the [ENTER] key to exit...");
            Console.ReadLine();
            Environment.Exit(errCode);
        }
    }
}