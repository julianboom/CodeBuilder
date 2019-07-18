using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextBuilder
{
    public partial class Form_Code : Form
    {
        public Form_Code()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            this.btnGenerate.Enabled = false;
            if (!this.textBox_dir.Text.IsPath())
            {
                MessageBox.Show("项目目录为空或不合法!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.btnGenerate.Enabled = true;
                return;
            }

            AddDirectory();
            AddNameSpace();

            System.Threading.Thread th = new System.Threading.Thread(CreateToDir);
            th.Start();
        }
        private void AddDirectory()
        {
            new Common.Config_Directory().Add(new Model.ConfigDirectory() { Name = this.textBox_dir.Text.Trim() });
        }

        private void AddNameSpace()
        {
            new Common.Config_NameSpace().Add(new Model.ConfigNameSpace()
            {
                Name1 = "",
                Name2 = ""
            });
        }

        private void CreateToDir()
        {
            List<TreeNode> NodeList = MainForm.form_Database.GetTreeView1Selected();
            if (NodeList.Count == 0)
            {
                return;
            }
            TreeNode serverNode = MainForm.form_Database.GetRoot(NodeList.First());
            TreeNode dbNode = NodeList.First().Parent.Parent;

            List<Model.BuilderMethods> methods = new List<Model.BuilderMethods>();


            Model.Servers server = (Model.Servers)((Model.TreeNodeTag)serverNode.Tag).Tag;
            Business.CreateCode CreateCode = new Business.CreateCode(server.Type);
            Model.CodeCreate param = new Model.CodeCreate();

            param.DbName = ((Model.TreeNodeTag)dbNode.Tag).Tag.ToString();
            param.NameSpace = new Common.Config_NameSpace().GetDefault().Name1;
            param.NameSpace1 = new Common.Config_NameSpace().GetDefault().Name2;
            param.ServerID = server.ID;
            param.BuilderType = Model.BuilderType.Default;
            param.MethodList = methods;
            param.CNSC = new Common.Config_NameSpaceClass().GetDefault();

            Business.CreateCode CreateCodeInstince = new Business.CreateCode(server.Type);

            
            StreamWriter sw;
            string FileName = string.Empty;
            var utf8WithBom = new System.Text.UTF8Encoding(true);  
            foreach (TreeNode node in NodeList)
            {
                param.TableName = ((Model.TreeNodeTag)node.Tag).Tag.ToString();
                param.ClassName = param.TableName;
                List<Model.ConfigTemplate> templateList = new Common.Config_Template().GetAll();
                for (int i = 0; i < templateList.Count; i++)
                {
                    var item = templateList[i];

                        if (item.IsActive == "是")
                        {
                            item.TargetName = item.TargetName.Replace("[ClassName]", param.ClassName);
                            item.DirectoryName = item.DirectoryName.Replace("[ClassName]", param.ClassName);
                            FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\{2}\\{3}", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1), item.DirectoryName, item.TargetName));
                            //sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) + item.TargetPostfix + FileName.Substring(FileName.LastIndexOf(".")));
                            sw = new StreamWriter(FileName.Substring(0, FileName.LastIndexOf(".")) + item.TargetPostfix + FileName.Substring(FileName.LastIndexOf(".")), false, utf8WithBom);
                            //sw.Write(CreateCodeInstince.GetModelClass(param));
                            sw.Write(CreateCodeInstince.GetBuilderClass(param, item.TemplateName));
                            sw.Close();
                            sw.Dispose();
                            lbMessage.Text = string.Format("生成文件:{0}", FileName);
                        }

                }
                /*//生成实体类
                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\{2}\\{3}.cs", this.textBox_dir.Text, param.NameSpace+(param.NameSpace1.IsNullOrEmpty() ? "" : "."+param.NameSpace1), param.CNSC.Model.Substring(param.CNSC.Model.LastIndexOf(".")+1),  param.ClassName));
                sw = File.CreateText(FileName);
                //sw.Write(CreateCodeInstince.GetModelClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param,"Entity.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);

                //生成数据类
                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\{2}\\{3}.cs", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1), param.CNSC.Data.Substring(param.CNSC.Data.LastIndexOf(".") + 1),  param.ClassName));
                sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) + "DAL" + FileName.Substring(FileName.LastIndexOf(".")));
                //sw.Write(CreateCodeInstince.GetDataClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "DAL.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);


                //生成业务类
                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\{2}\\{3}.cs", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1), param.CNSC.Business.Substring(param.CNSC.Business.LastIndexOf(".") + 1),  param.ClassName));
                sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) + "BLL" + FileName.Substring(FileName.LastIndexOf(".")));
                //sw.Write(CreateCodeInstince.GetBusinessClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "BLL.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);


                //生成接口类
                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\{2}\\{3}.cs", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1), param.CNSC.Interface.Substring(param.CNSC.Interface.LastIndexOf(".") + 1), param.ClassName));
                sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) + "IDAL" + FileName.Substring(FileName.LastIndexOf(".")));
                //sw.Write(CreateCodeInstince.GetInterfaceClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "IDAL.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);


                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\{2}\\{3}.cs", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1), param.CNSC.Interface.Substring(param.CNSC.Interface.LastIndexOf(".") + 1), param.ClassName));
                sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) + "IDAL" + FileName.Substring(FileName.LastIndexOf(".")));
                //sw.Write(CreateCodeInstince.GetInterfaceClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "IDAL.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);

                 
                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\json\\ColumnName.json", this.textBox_dir.Text));
                sw = File.CreateText(FileName);
                //sw.Write(CreateCodeInstince.GetInterfaceClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "Json.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);
                

                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\Controller\\{2}.cs", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1),  param.ClassName));
                sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) + "Controller" + FileName.Substring(FileName.LastIndexOf(".")));
                //sw.Write(CreateCodeInstince.GetInterfaceClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "Controller.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);
                //生成Json
                
                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\Views\\Index.cshtml", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1)));
                sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) +  FileName.Substring(FileName.LastIndexOf(".")));
                //sw.Write(CreateCodeInstince.GetInterfaceClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "Index.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);
                
                FileName = Common.Func.ExistsDirectory(string.Format("{0}\\{1}\\Views\\Edit.cshtml", this.textBox_dir.Text, param.NameSpace + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1)));
                sw = File.CreateText(FileName.Substring(0, FileName.LastIndexOf(".")) + FileName.Substring(FileName.LastIndexOf(".")));
                //sw.Write(CreateCodeInstince.GetInterfaceClass(param));
                sw.Write(CreateCodeInstince.GetBuilderClass(param, "Edit.txt"));
                sw.Close();
                sw.Dispose();
                lbMessage.Text = string.Format("生成文件:{0}", FileName);*/



            }
            MessageBox.Show("生成完成!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.lbMessage.Text = "生成已完成";
            this.btnGenerate.Enabled = true;
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            this.folderBrowserDialog1.ShowDialog();
            this.textBox_dir.Text = this.folderBrowserDialog1.SelectedPath;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
