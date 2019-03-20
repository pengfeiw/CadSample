using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

///There are two ways to access an object, one is using transcation manager and another is using open and close method.
///Whenever we want to access an object, we must open the object at first(Read, Write, Notify).
///This example show how to access the object usging two different ways.
///I suggest everyone use the transaction rather than the open and close method,Beacause the open-close casuse  error easier..
///-----------wpf.
namespace OpenAndCloseObject
{
    public class Class1
    {
        [CommandMethod("AddCircle1")]
        public static void AddCircleByTranscation()
        {
            Document curDoc = Application.DocumentManager.MdiActiveDocument;
            using (Transaction trans = curDoc.TransactionManager.StartTransaction())
            {
                BlockTable blk = trans.GetObject(curDoc.Database.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord btrM = trans.GetObject(blk[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Circle c = new Circle(new Point3d(0, 0, 0), Vector3d.ZAxis, 100);
                btrM.AppendEntity(c);
                trans.AddNewlyCreatedDBObject(c, true);
                trans.Commit();   //Commit the changes. Don't forget to add this line.
            }
        }

        /// <summary>
        /// The circle created can not be edited,The reason haven't been found yet.
        /// </summary>
        [CommandMethod("AddCircle2")]
        public static void AddCircleByOpenClose()
        {
            // Get the current document and database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            //Open the object.
            BlockTable blk = acCurDb.BlockTableId.Open(OpenMode.ForRead) as BlockTable;
            BlockTableRecord btrM = blk[BlockTableRecord.ModelSpace].Open(OpenMode.ForWrite) as BlockTableRecord;

            Circle c = new Circle(new Point3d(0, 0, 0), Vector3d.ZAxis, 100);
            btrM.AppendEntity(c);

            //Don't forget to close and dispos the Object.
            blk.Close();   
            blk.Dispose();

            btrM.Close();
            btrM.Dispose();
        }
    }
}
