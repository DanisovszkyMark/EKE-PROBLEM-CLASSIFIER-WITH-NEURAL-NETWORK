using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepetitionFilter
{
    class Program
    {
        private static List<string> lines = new List<string>();
        private static string fromPath = "SSA_8";
        private static string toPath = "SSA";

        static void Main(string[] args)
        {
            Console.WriteLine("A folyamat megkezdéséhez nyomj [ENTER]-t");
            Console.ReadLine();

            StreamReader sr = new StreamReader(fromPath + ".txt");
            string actualLine = "";

            Console.WriteLine("Kimenet előállítása...");
            while (!sr.EndOfStream)
            {
                actualLine = sr.ReadLine();
                if (!lines.Contains(actualLine)) lines.Add(actualLine);
            }
            sr.Close();

            Console.WriteLine("Fájl elkészítése...");
            StreamWriter sw = new StreamWriter(toPath + "_" + lines.Count + ".txt");
            for (int i = 0; i < lines.Count; i++)
            {
                sw.WriteLine(lines[i]);
            }
            sw.Close();

            Console.WriteLine("Folyamat kész!");
            Console.ReadLine();
        }
    }
}
