using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Colors;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using KpblcCadCore.ViewModels;
using KpblcCadCore.Views.Windows;
using KpblcExtensions.Enums;
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
            ObjectId att = ObjectId.Null;

            string attValue = string.Empty;

            BlockReference blockRef = blockRefId.Open(OpenMode.ForRead) as BlockReference;

            string tag = "ATT_TAG";

            foreach (ObjectId attRefId in blockRef.AttributeCollection)
            {
                AttributeReference attRef = attRefId.Open(OpenMode.ForRead) as AttributeReference;
                if (attRef.Tag.Equals(tag, StringComparison.InvariantCultureIgnoreCase))
                {
                    att = attRefId;
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

            if (vm.WinResult != WindowResult.OK)
            {
                return;
            }

            using (Transaction trans = doc.TransactionManager.StartTransaction())
            {
                if (vm.PaintBlockToBlue == true)
                {

                    // ToDo Текущее положение дел вываливает ошибку. Исправить!
                    blockRef.UpgradeOpen();
                    //blockRef.Color = Color.FromColorIndex(ColorMethod.ByAci, 5);
                    blockRef.ColorIndex = 5;
                }

                if (vm.PaintAttributeToRed == true)
                {
                    AttributeReference refAttr = trans.GetObject(att, OpenMode.ForWrite, false, true) as AttributeReference;
                    if (refAttr != null)
                    {
                        refAttr.ColorIndex = 1;
                    }
                }

                trans.Commit();
            }
        }
    }
}
