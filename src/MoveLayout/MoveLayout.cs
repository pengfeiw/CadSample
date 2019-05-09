using System;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;

namespace MoveLayout
{
    public class MoveLayout
    {
        /// <summary>
        /// 切换到innerBlock所在的layout（布局空间或者模型空间）
        /// </summary>
        public static  void SwitchToLayout(BlockReference innerBlock, Transaction tr)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            LayoutManager acLayoutMgr = LayoutManager.Current;

            BlockTableRecord blkTblRec = tr.GetObject(innerBlock.OwnerId, OpenMode.ForRead) as BlockTableRecord;
            Layout objLayout = tr.GetObject(blkTblRec.LayoutId, OpenMode.ForRead) as Layout;
            if (objLayout != null)
            {
                acLayoutMgr.CurrentLayout = objLayout.LayoutName;
            }
        }
    }
}
