using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;

namespace ChangeGridAndSnap
{
    public class Class1
    {
        [CommandMethod("ChangeGridAndSnap")]
        public static void ChangeGridAndSnap()
        {
            // Get the current database
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;

            // Start a transaction
            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Open the active viewport
                ViewportTableRecord acVportTblRec;
                acVportTblRec = acTrans.GetObject(acDoc.Editor.ActiveViewportId,
                                                  OpenMode.ForWrite) as ViewportTableRecord;

                // Turn on the grid for the active viewport
                acVportTblRec.GridEnabled = true;

                // Adjust the spacing of the grid to 1, 1
                acVportTblRec.GridIncrements = new Point2d(1, 1);

                // Turn on the snap mode for the active viewport
                acVportTblRec.SnapEnabled = true;

                // Adjust the snap spacing to 0.5, 0.5
                acVportTblRec.SnapIncrements = new Point2d(0.5, 0.5);

                // Change the snap base point to 1, 1
                acVportTblRec.SnapBase = new Point2d(1, 1);

                // Change the snap rotation angle to 30 degrees (0.524 radians)
                acVportTblRec.SnapAngle = 0.524;

                // Update the display of the tiled viewport
                acDoc.Editor.UpdateTiledViewportsFromDatabase();

                // Commit the changes and dispose of the transaction
                acTrans.Commit();
            }
        }
    }
}
