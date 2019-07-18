using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NextBuilder
{
    public partial class Form_SetTemplate : Form
    {
        public Form_SetTemplate()
        {
            InitializeComponent();
        }
        private Common.Config_Template Info = new Common.Config_Template();
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Model.ConfigTemplate template = GetModel();
            if (template != null)
            {
                if (Info.Add(template))
                {
                    MessageBox.Show("添加成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowList();
                }
                else
                {
                    MessageBox.Show("添加失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        private Model.ConfigTemplate GetModel()
        {
            string templateName = this.tbTemplateName.Text;
            string directoryName = this.tbDirectoryName.Text;
            string targetName = this.tbTargetName.Text;
            string targetPostfix = this.tbTargetPostfix.Text;
            string isActive = this.cbIsActive.Text;

            if (templateName.IsNullOrEmpty())
            {
                MessageBox.Show("模板名不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            if (directoryName.IsNullOrEmpty())
            {
                MessageBox.Show("目录名不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
            if (targetName.IsNullOrEmpty())
            {
                MessageBox.Show("生成文件名不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

            Model.ConfigTemplate template = new Model.ConfigTemplate();
            template.TemplateName = templateName.Trim();
            template.DirectoryName = directoryName.Trim();
            template.TargetName = targetName.Trim();
            template.TargetPostfix = targetPostfix.Trim();
            template.IsActive = isActive;
            return template;
        }

        private void Form_SetTemplate_Load(object sender, EventArgs e)
        {
            ShowList();
        }
        private void ShowList()
        {
            this.listView1.Items.Clear();
            var nlist = Info.GetAll();
            foreach (var list in nlist)
            {
                this.listView1.Items.Add(new ListViewItem(new string[] { 
                    list.TemplateName,
                    list.DirectoryName,
                    list.TargetName,
                    list.TargetPostfix,
                    list.IsActive
                }));
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            var items = listView1.SelectedItems;
            if (items.Count == 0)
                return;
            var item = items[0];
            Model.ConfigTemplate cnsc = GetModel();
            if (cnsc != null)
            {
                if (Info.Save(cnsc, item.SubItems[0].Text))
                {
                    MessageBox.Show("保存成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ShowList();
                }
                else
                {
                    MessageBox.Show("保存失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.tbTemplateName.Text.IsNullOrEmpty())
            {
                MessageBox.Show("您没有选择要删除的项!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Info.Delete(this.tbTemplateName.Text.Trim()))
            {
                MessageBox.Show("删除成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowList();
            }
            else
            {
                MessageBox.Show("删除失败!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }

        private void btnFresh_Click(object sender, EventArgs e)
        {
            ShowList();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = listView1.SelectedItems;
            if (items.Count == 0)
                return;
            var item = items[0];
            this.tbTemplateName.Text = item.SubItems[0].Text;
            this.tbDirectoryName.Text = item.SubItems[1].Text;
            this.tbTargetName.Text = item.SubItems[2].Text;
            this.tbTargetPostfix.Text = item.SubItems[3].Text;
            this.cbIsActive.Text = item.SubItems[4].Text;
        }
    }
}
