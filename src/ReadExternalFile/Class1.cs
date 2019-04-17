using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Runtime;
using System.IO;
using Autodesk.AutoCAD.Geometry;

/**
 * This example use Database.ReadDwgFile and Database.DxfIn to read .dxf and .dwg file into memory. 
 * Than make the all entities of ModelSpace(BlockTableRecord) into a block, and than insert in the database of the current active document. 
 */
namespace ReadExternalFile
{
    public class ReadExternalFile
    {
        static ObjectId id = new ObjectId();
        static ObjectId id2 = new ObjectId();
        [CommandMethod("test")]
        public static void readFileAsBlock()
        {
            string filename = @"D:\工作代码\test\drawing1.dwg";
            string filename2 = @"D:\工作代码\test\drawing2.dxf";
            id = createBlockFromFile(filename);
            id2 = createBlockFromFile(filename2);
            insertBlock(id, new Point3d(0, 0, 0));
            insertBlock(id, new Point3d(1000, 1000, 0));
            insertBlock(id2, new Point3d(1500, 0, 0));
            insertBlock(id2, new Point3d(-2000, 1000, 0));
        }
        [CommandMethod("insertBlock")]
        public static void insertBlockTest()
        {
            insertBlock(id, new Point3d(0, 0, 0));
            insertBlock(id, new Point3d(1000, 1000, 0));
            insertBlock(id2, new Point3d(1500, 0, 0));
            insertBlock(id2, new Point3d(-2000, 1000, 0));
        }

        /// <summary>
        /// Read dwg and dxf file into a memory, than  make all entities of ModelSpace into a block.
        /// </summary>
        /// <param name="cadPath">The file is readed into memory which should be a .dxf file or .dwg file</param>
        /// <returns></returns>
        public static ObjectId createBlockFromFile(string cadPath)
        {
            if (File.Exists(cadPath))
            {
                Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
                Database acDb = acDoc.Database;
                Database scDb = new Database(false, true);
                using (scDb)
                {
                    //Database scDb = new Database(true, true);
                    string fileExtension = System.IO.Path.GetExtension(cadPath);
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(cadPath);  //use fileName as blockName.
                    if (fileExtension == ".dxf")
                        scDb.DxfIn(cadPath, null);
                    else if (fileExtension == ".dwg")
                        scDb.ReadDwgFile(cadPath, FileOpenMode.OpenForReadAndReadShare, false, null);
                    else
                    {
                        //scDb.Dispose();
                        return new ObjectId();
                    }
                    using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
                    {
                        string blockName = fileName;
                        BlockTableRecord btrForInsert = new BlockTableRecord();
                        BlockTable acBlockTable = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                        //prevent block name repetition
                        int nameSuffix = 0;
                        while (acBlockTable.Has(blockName))
                        {
                            blockName = fileName + nameSuffix.ToString();
                            nameSuffix++;
                        }

                        btrForInsert.Name = blockName;

                        acBlockTable.UpgradeOpen();
                        ObjectId idForInsert = acBlockTable.Add(btrForInsert);
                        acTrans.AddNewlyCreatedDBObject(btrForInsert, true);
                        acBlockTable.DowngradeOpen();

                        using (Transaction scTrans = scDb.TransactionManager.StartTransaction())
                        {
                            BlockTable scBlockTable = scTrans.GetObject(scDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                            BlockTableRecord scModelSpace = scTrans.GetObject(scBlockTable[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;

                            foreach (ObjectId id in scModelSpace)
                            {
                                Entity ent = scTrans.GetObject(id, OpenMode.ForRead) as Entity;
                                if (ent != null)
                                {
                                    Entity newEnt = ent.Clone() as Entity;
                                    btrForInsert.AppendEntity(newEnt);
                                    acTrans.AddNewlyCreatedDBObject(newEnt, true);
                                }
                            }
                            scModelSpace.Dispose();
                            scTrans.Commit();
                        }
                        acTrans.Commit();
                        //scDb.Dispose();
                        return idForInsert;
                    }
                }
            }
            else
            {
                return new ObjectId();
            }
        }

        public static void insertBlock(ObjectId id, Point3d insertPoint)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acDb = acDoc.Database;

            using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;

                BlockTableRecord msBlkTbl = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

                BlockReference bref = new BlockReference(insertPoint, id);

                msBlkTbl.AppendEntity(bref);

                acTrans.AddNewlyCreatedDBObject(bref, true);
                acTrans.Commit();
            }
        }

        public static void insertBlock(string blockName, Point3d insertPoint)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acDb = acDoc.Database;

            ObjectId blockId = new ObjectId();

            using (Transaction acTrans = acDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl = acTrans.GetObject(acDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                blockId = acBlkTbl[blockName];
            }

            insertBlock(blockId, insertPoint);
        }
    }
}
