using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;

namespace IterateCollection
{
    public class Class1
    {
        [CommandMethod("IterateLayer")]
        public void iterateLayer()
        {
            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // This example returns the layer table for the current database
                LayerTable acLyrTbl;
                acLyrTbl = acTrans.GetObject(acCurDb.LayerTableId,
                                             OpenMode.ForRead) as LayerTable;

                // Step through the Layer table and print each layer name
                foreach (ObjectId acObjId in acLyrTbl)
                {
                    LayerTableRecord acLyrTblRec;
                    acLyrTblRec = acTrans.GetObject(acObjId,
                                                    OpenMode.ForRead) as LayerTableRecord;
                    acDoc.Editor.WriteMessage("\n" + acLyrTblRec.Name);
                }

                acTrans.Commit();
            }
        }

        [CommandMethod("IterateEntity")]
        public void iterateEntity()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database curDb = doc.Database;
            Editor ed = doc.Editor;

            using (Transaction trans = curDb.TransactionManager.StartTransaction())
            {
                BlockTable bt = trans.GetObject(curDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord modelSpaceRecord = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
                BlockTableRecord paperSpaceRecord = trans.GetObject(bt[BlockTableRecord.PaperSpace], OpenMode.ForRead) as BlockTableRecord;
                ed.WriteMessage("模型空间:\n");
                foreach (ObjectId id in modelSpaceRecord)
                {
                    ed.WriteMessage(id.ObjectClass.Name + "\n");
                }
                ed.WriteMessage("布局空间:\n");
                foreach (ObjectId id in paperSpaceRecord)
                {
                    ed.WriteMessage(id.ObjectClass.Name + "\n");
                }
                trans.Commit();
            }
        }

    }
}
