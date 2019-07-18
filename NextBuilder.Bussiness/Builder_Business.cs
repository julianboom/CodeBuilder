using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace NextBuilder.Business
{
    internal class Builder_Business
    {
        private IData.ICreateCode createInstance;
        private IData.IDatabase databaseInstance;
        private Import import;
        public Builder_Business(Model.DatabaseType dbType)
        {
            this.createInstance = Factory.Factory.CreateCreateCodeInstance(dbType);
            this.databaseInstance = Factory.Factory.CreateDatabaseInstance(dbType);
            this.import = new Import(dbType);
        }

        /// <summary>
        /// 得到业务层代码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetBusinessClass(Model.CodeCreate param)
        {
            Model.Servers server = Common.Config.GetServer(param.ServerID);
            if (server == null)
            {
                return string.Empty;
            }
            List<Model.Fields> fields = databaseInstance.GetFields(server.ID, param.DbName, param.TableName);

            //自增列
            var Identitys = fields.Where(p => p.IsIdentity);
            var NotIdeneitys = fields.Where(p => !p.IsIdentity);
            bool HasIdentity = Identitys.Count() > 0;

            //主键
            var Primarykeys = fields.Where(p => p.IsPrimaryKey);
            var NotPrimarykeys = fields.Where(p => !p.IsPrimaryKey);
            bool HasPrimarykey = Primarykeys.Count() > 0;

/*StringBuilder business = new StringBuilder(import.GetImport_Business());

            business.Append("namespace " + param.NameSpace + (param.NameSpace.IsNullOrEmpty() ? "" : ".") + param.CNSC.Business + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1) + "\r\n");
            business.Append("{\r\n");
            business.Append("\tpublic class " + param.ClassName + "BLL : BaseBLL<"+param.ClassName+">\r\n");
            business.Append("\t{\r\n");
            business.Append("\t\tprivate " + "I" + param.ClassName + "DAL " + param.ClassName[0].ToString().ToLower() + param.ClassName.Substring(1) + "DAL" + ";\r\n");
            business.Append("\t\tpublic " + param.ClassName + "BLL():: base()\r\n");
            business.Append("\t\t{\r\n");
            business.Append("\t\t\tbase.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);\r\n");
            business.Append("\t\t\tthis." + param.ClassName[0].ToString().ToLower() + param.ClassName.Substring(1) + "DAL = (I" + param.ClassName + "DAL)base.baseDal;\r\n");
            business.Append("\t\t}\r\n");

            business.Append("\t}\r\n");
            business.Append("}\r\n");*/

            string filePath=System.IO.Directory.GetCurrentDirectory()+"\\Template\\BLL.txt";
                        string value = "";

                FileStream fs = null;
                StreamReader sr = null;
                try
                {
                    if (!File.Exists(filePath)) 
                        return string.Empty;
                    fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    sr = new StreamReader(fs, System.Text.Encoding.UTF8);
                    value = sr.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
                finally
                {
                    if (sr != null)
                        sr.Close();
                    if (fs != null)
                        fs.Close();
                }

            string result =Engine.Razor.RunCompile(value, Guid.NewGuid().ToString(), null ,new { param=param });
                                               
            return result.ToString();

        }
    }
}
