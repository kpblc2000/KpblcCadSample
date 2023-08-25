using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace KpblcCadSample.CadLisp
{
    public static class PurgeLayersLisp
    {
        /// <summary> Удаление слоев </summary>
        /// <param name="rb">В лиспе - список масок имен слоев, которые надо оставить в живых. Обрабатываются только * и #</param>
        /// Пример: (purge-layers-except-mask '("abc*" "d#ef"))
        [LispFunction("purge-layers-except-mask")]
        public static ResultBuffer PurgeLayersExceptMaskLisp(ResultBuffer rb)
        {
            TypedValue[] args = rb.AsArray();
            List<string> usedLayers = new List<string>() { "0" };
            List<string> maskLayers = new List<string>();

            foreach (TypedValue item in args)
            {
                if (item.TypeCode == (int)LispDataType.Text)
                {
                    maskLayers.Add(item.Value.ToString()
                        .ToUpper()
                        .Replace("|", (char)92 + "|")
                        .Replace(".", (char)92 + ".")
                        .Replace("*", ".*")
                        .Replace("#", "[0-9]")
                    );
                }
            }

            Database dBase = HostApplicationServices.WorkingDatabase;
            using (Transaction trans = dBase.TransactionManager.StartTransaction())
            {
                using (BlockTable blockTable = trans.GetObject(dBase.BlockTableId, OpenMode.ForRead) as BlockTable)
                {
                    foreach (ObjectId blockRecId in blockTable)
                    {
                        using (BlockTableRecord blockDef = trans.GetObject(blockRecId, OpenMode.ForRead) as BlockTableRecord)
                        {
                            foreach (ObjectId id in blockDef)
                            {
                                using (Entity entity = trans.GetObject(id, OpenMode.ForRead, false, true) as Entity)
                                {
                                    if (!usedLayers.Contains(entity.Layer.ToUpper()))
                                    {
                                        usedLayers.Add(entity.Layer.ToUpper());
                                    }
                                }
                            }
                        }
                    }
                }

                List<string> erasedLayerNamesList = new List<string>();

                using (LayerTable layerTable = trans.GetObject(dBase.LayerTableId, OpenMode.ForRead) as LayerTable)
                {
                    foreach (ObjectId layerId in layerTable)
                    {
                        using (LayerTableRecord layerEntity = trans.GetObject(layerId, OpenMode.ForRead) as LayerTableRecord)
                        {
                            string layerName = layerEntity.Name;
                            
                            if (!usedLayers.Contains(layerName.ToUpper())
                                && !maskLayers.Where(o => Regex.IsMatch(layerName.ToUpper(), o)).Any()
                                )
                            {
                                layerEntity.UpgradeOpen();
                                try
                                {
                                    layerEntity.Erase();
                                    erasedLayerNamesList.Add(layerName);
                                }
                                catch
                                {
                                    layerEntity.DowngradeOpen();
                                }
                            }
                        }
                    }
                }
                trans.Commit();

                ResultBuffer res = new ResultBuffer();
                res.Add(new TypedValue((int)LispDataType.ListBegin));
                foreach (string item in erasedLayerNamesList)
                {
                    res.Add(new TypedValue((int)LispDataType.Text, item));
                }
                res.Add(new TypedValue((int)LispDataType.ListEnd));

                return res;
            }
        }
    }
}
