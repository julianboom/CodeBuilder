using NextBuilder.Model;
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
    public partial class Form_AddDatabase : Form
    {
        public Form_AddDatabase()
        {
            InitializeComponent();
        }
        private DatabaseType DatabaseType = DatabaseType.Empty;
        private void button1_Click(object sender, EventArgs e)
        {
            this.DatabaseType = DatabaseType.MySql;
            switch (this.DatabaseType)
            {

                case Model.DatabaseType.MySql:
                    this.Close();
                    Form_AddDatabase_MySql famysql = new Form_AddDatabase_MySql(this.DatabaseType);
                    famysql.ShowDialog();
                    break;
            }
        }

        public static void AddServerToXml(Model.ConfigServers cs)
        {
            new Common.Config_Servers().Add(cs);
        }
    }
}
