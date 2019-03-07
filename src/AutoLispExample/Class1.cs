using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;

namespace AutoLispExample
{
    public class Class1
    {
        /// <summary>
        /// 使用方法：启动CAD，加载项目，命令行输入(displayfullname "First" "Last")。
        /// </summary>
        [LispFunction("DisplayFullName")]
        public static void DisplayFullName(ResultBuffer rbArgs)
        {
            if (rbArgs != null)
            {
                string strVal1 = "";
                string strVal2 = "";

                int nCnt = 0;
                foreach (TypedValue rb in rbArgs)
                {
                    if (rb.TypeCode == (int)Autodesk.AutoCAD.Runtime.LispDataType.Text)
                    {
                        switch (nCnt)
                        {
                            case 0:
                                strVal1 = rb.Value.ToString();
                                break;
                            case 1:
                                strVal2 = rb.Value.ToString();
                                break;
                        }
                        nCnt = nCnt + 1;
                    }
                }

                Application.DocumentManager.MdiActiveDocument.Editor.
                   WriteMessage("\nName: " + strVal1 + " " + strVal2);
            }
        }
    }
}
