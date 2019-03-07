using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using System.Drawing;

namespace ControlWindow
{
    public class Class1
    {
        [CommandMethod("ChangeWindowPosition")]
        public void ChangeWindowLocation()
        {
            Point lt = new Point(0, 0);
            Application.MainWindow.Location = lt;
            Size size = new Size(400, 400);
            Application.MainWindow.Size = size;
        }
    }
}
