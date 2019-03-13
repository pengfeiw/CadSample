using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;

namespace LockDoc
{
    public class Class1
    {
        [CommandMethod("LockDoc", CommandFlags.Session)]
        public static void LockDoc()
        {
            // Create a new drawing
            DocumentCollection acDocMgr = Application.DocumentManager;
            Document acNewDoc = acDocMgr.Add("acad.dwt");
            Database acDbNewDoc = acNewDoc.Database;

            // Lock the new document
            using (DocumentLock acLckDoc = acNewDoc.LockDocument())
            {
                // Start a transaction in the new database
                using (Transaction acTrans = acDbNewDoc.TransactionManager.StartTransaction())
                {
                    // Open the Block table for read
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acDbNewDoc.BlockTableId,
                                                 OpenMode.ForRead) as BlockTable;

                    // Open the Block table record Model space for write
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    // Create a circle with a radius of 3 at 5,5
                    Circle acCirc = new Circle();
                    acCirc.SetDatabaseDefaults();
                    acCirc.Center = new Point3d(5, 5, 0);
                    acCirc.Radius = 3;

                    // Add the new object to Model space and the transaction
                    acBlkTblRec.AppendEntity(acCirc);
                    acTrans.AddNewlyCreatedDBObject(acCirc, true);

                    // Save the new object to the database
                    acTrans.Commit();
                }

                // Unlock the document
            }

            // Set the new document current
            acDocMgr.MdiActiveDocument = acNewDoc;
        }

        //The follow code meet an Error: elovkViolation,But I don't konw why.
        [CommandMethod("UnLockDoc", CommandFlags.Session)]
        public static void UnLockDoc()
        {
            DocumentCollection docMgr = Application.DocumentManager;
            Document newDoc = docMgr.Add("acad.dwt");
            Database acDbNewDoc = newDoc.Database;

            docMgr.MdiActiveDocument = newDoc;

            using (Transaction trans = newDoc.TransactionManager.StartTransaction())
            {
                //docMgr.MdiActiveDocument = newDoc;
                
                BlockTable acBlkTbl = trans.GetObject(acDbNewDoc.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord brec = trans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Line lin = new Line(new Point3d(0, 0, 0), new Point3d(100, 100, 0));

                brec.AppendEntity(lin);
                trans.AddNewlyCreatedDBObject(lin, true);

                trans.Commit();
            }
        }
    }
}
