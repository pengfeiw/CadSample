using System;
using Autodesk.AutoCAD;
using Autodesk.AutoCAD.Interop;
using Autodesk.AutoCAD.Interop.Common;
using System.Runtime.InteropServices;

namespace ConsoleApplication1
{
    class Program
    {

        #region ProgID

        const string CAD2004_progID = "AutoCAD.Application.16";
        const string CAD2005_progID = "AutoCAD.Application.16.1";
        const string CAD2006_progID = "AutoCAD.Application.16.2";
        const string CAD2007_progID = "AutoCAD.Application.17";
        const string CAD2008_progID = "AutoCAD.Application.17.1";
        const string CAD2009_progID = "AutoCAD.Application.17.2";
        const string CAD2010_progID = "AutoCAD.Application.18";
        const string CAD2011_progID = "AutoCAD.Application.18.1";

        #endregion

        static void Main(string[] args)
        {
            const string progID = "AutoCAD.Application.18";
            startCad(progID);
        }

        public static void startCad(string progID)
        {
            AcadApplication acApp = null;
            try
            {
                acApp =
                  (AcadApplication)Marshal.GetActiveObject(progID);
            }
            catch
            {
                try
                {
                    Type acType =
                      Type.GetTypeFromProgID(progID);
                    acApp =
                      (AcadApplication)Activator.CreateInstance(
                        acType,
                        true
                      );
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Can not open AutoCAD!");
                }
            }
            if (acApp != null)
            {
                acApp.Visible = true;
                acApp.ActiveDocument.SendCommand("_MYCOMMAND ");
            }
        }
    }
}
