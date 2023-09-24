using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using KpblcCadCore.ViewModels;
using KpblcCadCore.Views.Windows;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpblcCadSample.CadCommands
{
    public static class PaintBlockCommad
    {
        [CommandMethod("paint-block")]
        public static void BlockCommad()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database dbase = doc.Database;

            SelectionFilter sFilter = new SelectionFilter(
                new TypedValue[2]
                {
                    new TypedValue(0, "INSERT"),
                    new TypedValue(66,1)
                }
                );

            PromptSelectionOptions psOptions = new PromptSelectionOptions();
            psOptions.MessageForAdding = "\nВыберите блок с атрибутом : ";
            psOptions.MessageForRemoval = "\nУдалить из выбора : ";
            psOptions.SingleOnly = true;

            PromptSelectionResult selResult = ed.GetSelection(psOptions, sFilter);

            if (selResult.Status != PromptStatus.OK)
            {
                return;
            }

            SelectionSet selSet = selResult.Value;

            ObjectId blockRefId = selSet.GetObjectIds()[0];

            string attValue = string.Empty;

            BlockReference blockRef = blockRefId.Open(OpenMode.ForRead) as BlockReference;

            string tag = "ATT_TAG";

            foreach (ObjectId attRefId in blockRef.AttributeCollection)
            {
                AttributeReference attRef = attRefId.Open(OpenMode.ForRead) as AttributeReference;
                if (attRef.Tag.Equals(tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    attValue = attRef.TextString;
                    break;
                }
            }

            BlockAttributeColoringViewModel vm = new BlockAttributeColoringViewModel();
            vm.AttributeText = attValue;

            PaintAttrAndBlockWindow win = new PaintAttrAndBlockWindow()
            {
                DataContext = vm,
            };

            Application.ShowModalWindow(win);
        }
    }
}
