using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace NextBuilder
{
    public partial class MainForm : DockContent
    {
        public static MainForm Instance = null;
        public static Form_Database form_Database = null;
        public static Form_Home form_Home = null;
        //public static Form_TemplateTree form_TemplateTree = null;
        public MainForm()
        {
            InitializeComponent();
            Instance = this;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            form_Database = new Form_Database();
            form_Database.Show(dockPanel1, DockState.DockLeft);

            form_Home = new Form_Home();
            form_Home.Show(dockPanel1);
            form_Home.Activate();
        }
        /// <summary>
        /// 显示起始页
        /// </summary>
        public void ShowHome()
        {
            if (form_Home == null)
            {
                form_Home = new Form_Home();
                form_Home.Show(dockPanel1);
            }

            form_Home.Activate();
        }

        private void TSMIAddServer_Click(object sender, EventArgs e)
        {
            Form_AddDatabase f_adddb = new Form_AddDatabase();
            f_adddb.ShowDialog();
        }

        private void TSMICancelServer_Click(object sender, EventArgs e)
        {
            //form_Database.RemoveServer();
        }

        private void TSMIExit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void TSBHome_Click(object sender, EventArgs e)
        {
            ShowHome();
        }

        /// <summary>
        /// 显示服务器资源管理器
        /// </summary>
        public void ShowServerList()
        {
            if (form_Database == null)
            {
                form_Database = new Form_Database();
                form_Database.Show(dockPanel1, DockState.DockLeftAutoHide);
            }
            form_Database.Activate();
        }

        private void TSBAddServer_Click(object sender, EventArgs e)
        {
            TSMIAddServer_Click(sender, e);
        }

        private void TSBCancelServer_Click(object sender, EventArgs e)
        {
            TSMICancelServer_Click(sender, e);
        }


        private void TSBExit_Click(object sender, EventArgs e)
        {
            TSMIExit_Click(sender, e);
        }
        /// <summary>
        /// 退出系统
        /// </summary>
        private void Exit()
        {
            Application.Exit();
        }

        private void TSMINameSpace_Click(object sender, EventArgs e)
        {
            Form_SetNameSpace fs = new Form_SetNameSpace();
            fs.ShowDialog();
        }

        private void TSMIClassSpace_Click(object sender, EventArgs e)
        {
            Form_SetNameSpaceClass fssc = new Form_SetNameSpaceClass();
            fssc.ShowDialog();
        }

        private void TSMITemplate_Click(object sender, EventArgs e)
        {
            Form_SetTemplate formTemplate = new Form_SetTemplate();
            formTemplate.ShowDialog();
        }
    }
}
