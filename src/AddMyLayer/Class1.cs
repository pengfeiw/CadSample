using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;

namespace AddMyLayer
{
    public class Class1
    {
        [CommandMethod("AddMylayer")]
        public void addMyLayer()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database curDb = doc.Database;
            using (Transaction trans = curDb.TransactionManager.StartTransaction())
            {
                LayerTable lt = trans.GetObject(curDb.LayerTableId, OpenMode.ForWrite) as LayerTable;
                // Check to see if MyLayer exists in the Layer table
                if (lt.Has("MyLayer") != true)
                {
                    // Open the Layer Table for write
                    lt.UpgradeOpen();

                    // Create a new layer table record and name the layer "MyLayer"
                    LayerTableRecord acLyrTblRec = new LayerTableRecord();
                    acLyrTblRec.Name = "MyLayer";

                    // Add the new layer table record to the layer table and the transaction
                    lt.Add(acLyrTblRec);
                    trans.AddNewlyCreatedDBObject(acLyrTblRec, true);

                    // Commit the changes
                    trans.Commit();
                }
            }
        }
    }
}
