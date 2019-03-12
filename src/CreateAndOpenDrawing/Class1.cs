using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;


namespace CreateAndOpenDrawing
{
    public class Class1
    {
        [CommandMethod("CreateDrawing")]
        public static void createDrawing()
        {
            // Specify the template to use, if the template is not found
            // the default settings are used.
            string strTemplatePath = "acad.dwt";
            DocumentCollection docs = Application.DocumentManager;
            Document doc = docs.Add("strTemplatePath");
            docs.MdiActiveDocument = doc;
        }

        [CommandMethod("OpenDrawing")]
        public static void OpenExitingDrawing()
        {
            string curDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fileDir = System.IO.Path.Combine(curDir, "resources");
            string filePath = System.IO.Path.Combine(fileDir, "openDrawing.dwg");
            DocumentCollection docMgr = Application.DocumentManager;
            if (System.IO.File.Exists(filePath))
            {
                Document doc = docMgr.Open(filePath, false);
                docMgr.MdiActiveDocument = doc;
            }
            else
            {
                docMgr.MdiActiveDocument.Editor.WriteMessage("要打开的目标文件：" + filePath + "不存在！");
            }
        }
    }
}
