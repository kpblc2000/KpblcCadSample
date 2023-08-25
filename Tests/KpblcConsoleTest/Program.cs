using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KpblcExtensions.Repository;

namespace KpblcConsoleTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string sourceFolder = @"C:\Autodesk\multiflag";
            string destFolder = @"C:\Autodesk\multiflag\copy";

            CacheRepository rep = new CacheRepository();
            rep.SyncronizeFolders(sourceFolder, destFolder, new string[] {".lsp"}, true);

            string sourceFileName= @"C:\Autodesk\multiflag\multiflag.fas";
            string destFileName= @"C:\Autodesk\multiflag\copy2\copy132232\multiflag.fas";

            rep.CopyFileOnDemand(sourceFileName, destFileName);

            Console.WriteLine("Press any key");
            Console.ReadKey();
        }
    }
}
