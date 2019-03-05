using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using  Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;

namespace AttachXref
{
    public class Class1
    {
        [CommandMethod("ListEntities")]
        public static void ListEntities()
        {
            // Get the current document and database, and start a transaction
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the Block table record for read
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                             OpenMode.ForRead) as BlockTable;

                // Open the Block table record Model space for read
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                OpenMode.ForRead) as BlockTableRecord;

                int nCnt = 0;
                acDoc.Editor.WriteMessage("\nModel space objects: ");

                // Step through each object in Model space and
                // display the type of object found
                foreach (ObjectId acObjId in acBlkTblRec)
                {
                    acDoc.Editor.WriteMessage("\n" + acObjId.ObjectClass.DxfName);
                    nCnt = nCnt + 1;
                }

                // If no objects are found then display a message
                if (nCnt == 0)
                {
                    acDoc.Editor.WriteMessage("\n No objects found");
                }

                // Dispose of the transaction
            }
        }
    }
}
