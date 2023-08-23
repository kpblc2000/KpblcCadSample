using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Autodesk.AutoCAD.Runtime;

namespace KpblcCadSample.CadCommands
{
    public static class AboutCommand
    {
        [CommandMethod("kpblc-about")]
        public static void AboutCadCommand()
        {
            MessageBox.Show("Тип о программе", "KpblcCadSample", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
