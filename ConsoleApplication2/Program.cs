using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication2
{
    class Program
    {
        public static void PrintFiles(string dir, UInt16 fieldSize)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] files = dirInfo.GetFiles();

            foreach (FileInfo file in files)

                Console.WriteLine(file.FullName /*+ file.Name.PadLeft(fieldSize)*/);
            DirectoryInfo[] dirs = dirInfo.GetDirectories();
            foreach (DirectoryInfo d in dirs)
                PrintFiles(d.FullName, (UInt16)(fieldSize + 4));
           
        }
        static void Main(string[] args)
        {
            PrintFiles("D:\\music", 0);

            Console.ReadLine();
        }
    }
}
