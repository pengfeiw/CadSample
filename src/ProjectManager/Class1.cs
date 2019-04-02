using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Newtonsoft.Json.Linq;

namespace ProjectManager
{
    public class ProjectManager
    {
        Dictionary<string, string> nodeToCommand = new Dictionary<string, string>()
        {
            //{"AddMyLayer", "AddMylayer"},
            //{"AutoLispExample","DisplayFullName"},
            //{"ChangeGridAndSnap", "ChangeGridAndSnap"},
            //{"ControlDocumentWindow","ChangeDrawWindow"},
            //{"ControlWindow", "ChangeWindowPosition"},
            //{"CreateAndEraseView1","CreateNamedView"},
            //{"CreateAndEraseView2", "EraseNamedView"},
            //{"CreateAndOpenDrawing1","CreateDrawing"},
            //{"CreateAndOpenDrawing2","OpenDrawing"}
        };
        internal UserControl treeContainer = null;
        internal TreeView tree = null;
        internal ProjectManager()
        {
            treeContainer = new UserControl();
            getDataFromJson();
            tree = new TreeView();
            treeContainer.Dock = DockStyle.Fill;
            treeContainer.Controls.Add(tree);
            tree.Dock = DockStyle.Fill;
            tree.ShowNodeToolTips = true;
            tree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(tree_NodeMouseDoubleClick);
            showDataOnTree();
        }

        private void getDataFromJson()
        {
            string curfile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string curDir = Path.GetDirectoryName(curfile);
            string jsonFile = Path.Combine(curDir, "LoadFiles.json");
            string readResult = string.Empty;
            using (StreamReader r = new StreamReader(jsonFile))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                readResult = jobj.ToString();
                foreach (var item in jobj.Properties())
                {
                    string dllName = item.Name + ".dll";
                    string dllPath = Path.Combine(curDir, dllName);
                    if (File.Exists(dllPath))
                    {
                        if (!nodeToCommand.Keys.Contains(item.Name))
                        {
                            nodeToCommand.Add(item.Name, item.Value.ToString().Trim('\"'));
                        }
                    }
                }
            }
        }

        private void showDataOnTree()
        {
            if (tree != null)
            {
                foreach (var key in nodeToCommand.Keys)
                {
                    tree.Nodes.Add(key);
                }
            }
        }

        void tree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            string command = nodeToCommand[e.Node.Text];
            acDoc.SendStringToExecute(command, true, false, true);
        }
    }
    public class Class1
    {
        public static Autodesk.AutoCAD.Windows.PaletteSet projectPanel = null;
        public static ProjectManager projectManager = null;

        [CommandMethod("ShowProject")]
        public void showProjectManager()
        {
            projectPanel = new Autodesk.AutoCAD.Windows.PaletteSet("CadDemo");
            projectManager = new ProjectManager();
            projectPanel.Add(".Net案例", projectManager.treeContainer);
            projectPanel.Visible = true;
            projectPanel.Dock = Autodesk.AutoCAD.Windows.DockSides.Left;
        }

        [CommandMethod("StartProject")]
        public void startProject()
        {
            //自动加载dll
            string curfile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string curDir = Path.GetDirectoryName(curfile);
            string jsonFile = Path.Combine(curDir, "LoadFiles.json");
            string readResult = string.Empty;
            Document acDoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Editor ed = acDoc.Editor;
            using (StreamReader r = new StreamReader(jsonFile))
            {
                var json = r.ReadToEnd();
                var jobj = JObject.Parse(json);
                readResult = jobj.ToString();
                foreach (var item in jobj.Properties())
                {
                    string dllName = item.Name + ".dll";
                    string dllPath = Path.Combine(curDir, dllName);
                    if (File.Exists(dllPath))
                    {
                        Assembly.LoadFrom(dllPath);
                    }
                    else
                    {
                        ed.WriteMessage("\n加载文件失败：" + dllPath);
                        continue;
                    }
                }
            }

            acDoc.SendStringToExecute("ShowProject ", true, false, true);
        }
    }
}
