using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace EraseObject
{
    /// <summary>
    /// 注意：To determine if a layer or another named object such as a block or text style can be erased, you should use the Purge method. 
    /// </summary>
    public class Class1
    {
        [CommandMethod("EraseMyLayer")]
        public void EraseMyLayer()
        {
            Document doc =  Application.DocumentManager.MdiActiveDocument;
            Database curDb = doc.Database;

            using (Transaction trans = curDb.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(curDb.LayerTableId, OpenMode.ForRead) as LayerTable;
                if (lt.Has("MyLayer"))
                {
                    LayerTableRecord myLayer = trans.GetObject(lt["myLayer"], OpenMode.ForWrite) as LayerTableRecord;
                    myLayer.Erase();
                    doc.Editor.WriteMessage("成功删除图层MyLayer.");
                }
                else
                    doc.Editor.WriteMessage("图层MyLayer不存在！");
                trans.Commit();
            }
        }

        [CommandMethod("EraseLine")]
        public void EraseAllLine()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database curDb = doc.Database;
            using (Transaction trans = curDb.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(curDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btrModelSpace = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                BlockTableRecord btrPaperSapce = trans.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForWrite) as BlockTableRecord;
                int eraseCntOfModel = 0, eraseCntOfPaper = 0;
                foreach (ObjectId id in btrModelSpace)
                {
                    if (id.ObjectClass.DxfName == "LINE")
                    {
                        Line ob = trans.GetObject(id, OpenMode.ForWrite) as Line;
                        ob.Erase();
                        eraseCntOfModel++;
                    }
                }
                foreach (ObjectId id in btrPaperSapce)
                {
                    if (id.ObjectClass.DxfName == "LINE")
                    {
                        Line ob = trans.GetObject(id, OpenMode.ForWrite) as Line;
                        ob.Erase();
                        eraseCntOfPaper++;
                    }
                }
                doc.Editor.WriteMessage("模型空间共删除{0}个Line", eraseCntOfModel);
                doc.Editor.WriteMessage("布局空间共删除{0}个Line", eraseCntOfPaper);
                trans.Commit();
            }
        }
    }
}
