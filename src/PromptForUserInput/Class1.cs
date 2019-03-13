using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace PromptForUserInput
{
    public class Class1
    {
        [CommandMethod("GetStringFromUser")]
        public static void GetStringFromUser()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter your name: ");
            pStrOpts.AllowSpaces = true;
            PromptResult pStrRes = acDoc.Editor.GetString(pStrOpts);

            Application.ShowAlertDialog("The name entered was: " +
                                        pStrRes.StringResult);
        }

        [CommandMethod("GetPointsFromUser")]
        public static void GetPointsFromuser()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointOptions pPointOpts = new PromptPointOptions("\nEnter start point: ");
            PromptPointResult pPointres = doc.Editor.GetPoint(pPointOpts);

            PromptPointOptions pPointOpts2 = new PromptPointOptions("\nEnter end point: ");
            PromptPointResult pPointres2 = doc.Editor.GetPoint(pPointOpts2);

            Point3d startPnt = pPointres.Value;
            Point3d endPnt = pPointres2.Value;

            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                BlockTable blkTbl = trans.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord blkTblRec = trans.GetObject(blkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                Line line = new Line(startPnt, endPnt);
                blkTblRec.AppendEntity(line);
                trans.AddNewlyCreatedDBObject(line, true);
                trans.Commit();
            }
        }

        [CommandMethod("GetKeywordFromUser")]
        public static void GetKeywordFromUser()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("");
            pKeyOpts.Message = "\nEnter an option ";
            pKeyOpts.Keywords.Add("Line");
            pKeyOpts.Keywords.Add("Circle");
            pKeyOpts.Keywords.Add("Arc");
            pKeyOpts.AllowNone = false;

            PromptResult pKeyRes = acDoc.Editor.GetKeywords(pKeyOpts);

            Application.ShowAlertDialog("Entered keyword: " +
                                        pKeyRes.StringResult);
        }

        [CommandMethod("GetKeywordFromUser2")]
        public static void GetKeywordFromUser2()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            PromptKeywordOptions pKeyOpts = new PromptKeywordOptions("");
            pKeyOpts.Message = "\nEnter an option ";
            pKeyOpts.Keywords.Add("Line");
            pKeyOpts.Keywords.Add("Circle");
            pKeyOpts.Keywords.Add("Arc");
            pKeyOpts.Keywords.Default = "Arc";
            pKeyOpts.AllowNone = true;

            PromptResult pKeyRes = acDoc.Editor.GetKeywords(pKeyOpts);

            Application.ShowAlertDialog("Entered keyword: " +
                                        pKeyRes.StringResult);
        }

        [CommandMethod("GetIntegerOrKeywordFromUser")]
        public static void GetIntegerOrKeywordFromUser()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;

            PromptIntegerOptions pIntOpts = new PromptIntegerOptions("");
            pIntOpts.Message = "\nEnter the size or ";

            // Restrict input to positive and non-negative values
            pIntOpts.AllowZero = false;
            pIntOpts.AllowNegative = false;

            // Define the valid keywords and allow Enter
            pIntOpts.Keywords.Add("Big");
            pIntOpts.Keywords.Add("Small");
            pIntOpts.Keywords.Add("Regular");
            pIntOpts.Keywords.Default = "Regular";
            pIntOpts.AllowNone = true;

            // Get the value entered by the user
            PromptIntegerResult pIntRes = acDoc.Editor.GetInteger(pIntOpts);

            if (pIntRes.Status == PromptStatus.Keyword)
            {
                Application.ShowAlertDialog("Entered keyword: " +
                                            pIntRes.StringResult);
            }
            else
            {
                Application.ShowAlertDialog("Entered value: " +
                                            pIntRes.Value.ToString());
            }
        }
    }
}
