using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace SaveDrawing
{
    public class Class1
    {
        [CommandMethod("SaveDrawing1")]
        public static void saveDrawing1()
        {
            //save the current drawing,if the current drawing is unnamed, rename it
            DocumentCollection docMgr = Application.DocumentManager;
            Document activeDoc = docMgr.MdiActiveDocument;
            string fileName = activeDoc.Name;
            object obj = Application.GetSystemVariable("DWGTITLED");
            if (System.Convert.ToInt16(obj) == 0)
            {
                string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                string fileDir = System.IO.Path.Combine(curDir, "resources");
                fileName = System.IO.Path.Combine(fileDir, "saveDrawing.dwg");
            }
            activeDoc.Database.SaveAs(fileName, true, DwgVersion.Current, activeDoc.Database.SecurityParameters);
        }

        [CommandMethod("SaveDrawing2")]
        public static void saveDrawing2()
        {
            //save the drawing if the drawing is changed. 
            object obj = Application.GetSystemVariable("DBMOD");
            if (System.Convert.ToInt16(obj) != 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Do you wish to save this drawing?",
                                "Save Drawing",
                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                System.Windows.Forms.MessageBoxIcon.Question)
                                == System.Windows.Forms.DialogResult.Yes)
                {
                    Document acDoc = Application.DocumentManager.MdiActiveDocument;
                    acDoc.Database.SaveAs(acDoc.Name, true, DwgVersion.Current, acDoc.Database.SecurityParameters);
                }
            }
        }
    }
}
