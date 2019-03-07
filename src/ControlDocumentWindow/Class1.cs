using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using System.Drawing;

namespace ControlDocumentWindow
{
    public class Class1
    {
        [CommandMethod("ChangeDrawWindow")]
        public void changeDocWindow()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            doc.Window.WindowState = Autodesk.AutoCAD.Windows.Window.State.Normal;
            doc.Window.Location = new Point(400, 400);
            doc.Window.Size = new Size(700, 1000);
        }
    }
}
