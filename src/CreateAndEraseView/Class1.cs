using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace CreateAndEraseView
{
    public class Class1
    {
        /// <summary>
        /// 为图添加视图，这里视图指的是从某个角度看(俯视图，正视图，侧视图)。
        /// </summary>
        [CommandMethod("CreateNamedView")]
        public static void CreateNamedView()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                ViewTable viewTable = trans.GetObject(db.ViewTableId, OpenMode.ForRead) as ViewTable;
                if (viewTable.Has("view2") == false)
                {
                    ViewTableRecord newViewRecord = new ViewTableRecord();
                    newViewRecord.Name = "view2";
                    viewTable.UpgradeOpen();
                    viewTable.Add(newViewRecord);
                    trans.AddNewlyCreatedDBObject(newViewRecord, true);
                    doc.Editor.SetCurrentView(newViewRecord);
                }
                trans.Commit();
            }
        }

        [CommandMethod("EraseNamedView")]
        public static void EraseNamedView()
        {
            // Get the current database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the View table for read
                ViewTable acViewTbl;
                acViewTbl = acTrans.GetObject(acCurDb.ViewTableId,
                                              OpenMode.ForRead) as ViewTable;

                // Check to see if the named view 'View1' exists
                if (acViewTbl.Has("View1") == true)
                {
                    // Open the View table for write
                    acViewTbl.UpgradeOpen();

                    // Get the named view
                    ViewTableRecord acViewTblRec;
                    acViewTblRec = acTrans.GetObject(acViewTbl["View1"],
                                                     OpenMode.ForWrite) as ViewTableRecord;

                    // Remove the named view from the View table
                    acViewTblRec.Erase();

                    // Commit the changes
                    acTrans.Commit();
                }

                // Dispose of the transaction
            }
        }
    }
}
