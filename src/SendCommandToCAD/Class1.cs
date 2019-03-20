using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

namespace SendCommandToCAD
{
    public class Class1
    {
        [CommandMethod("SendCommand")]
        public static void SendCommandToCAD()
        {
            Document curDoc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = curDoc.Editor;
            curDoc.SendStringToExecute("Circle 2,2,0 4 ", true, false, false);
            curDoc.SendStringToExecute("zoom e ", true, false, false);
        }
    }
}
