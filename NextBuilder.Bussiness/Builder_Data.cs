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
    internal class Builder_Data
    {
        private IData.ICreateCode createInstance;
        private IData.IDatabase databaseInstance;
        private Import import;
        public Builder_Data(Model.DatabaseType dbType)
        {
            this.createInstance = Factory.Factory.CreateCreateCodeInstance(dbType);
            this.databaseInstance = Factory.Factory.CreateDatabaseInstance(dbType);
            this.import = new Import(dbType);
        }
        /// <summary>
        /// 得到数据层代码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public string GetDataClass(Model.CodeCreate param)
        {
            Model.Servers server = Common.Config.GetServer(param.ServerID);
            if (server == null)
            {
                return string.Empty;
            }
            List<Model.Fields> fields = databaseInstance.GetFields(server.ID, param.DbName, param.TableName);
            /*StringBuilder data = new StringBuilder(import.GetImport_Data());

            data.Append("namespace " + param.NameSpace + (param.NameSpace.IsNullOrEmpty() ? "" : ".") + param.CNSC.Data + (param.NameSpace1.IsNullOrEmpty() ? "" : "." + param.NameSpace1) + "\r\n");
            data.Append("{\r\n");
            data.Append("\tpublic class " + param.ClassName + "DAL: BaseDALMySql<" + param.ClassName + "> , I" + param.ClassName + "DAL\r\n");
            data.Append("\t{\r\n");
            data.Append("\t\tpublic static " + param.ClassName + "DAL Instance\r\n");
            data.Append("\t\t{\r\n");
            data.Append("\t\t\tget\r\n");
            data.Append("\t\t\t{\r\n");
            data.Append("\t\t\t\treturn new " + param.ClassName + "DAL();\r\n");
            data.Append("\t\t\t}\r\n");
            data.Append("\t\t}\r\n");
            data.Append("\t\t/// <summary>\r\n");
            data.Append("\t\t/// 构造函数\r\n");
            data.Append("\t\t/// </summary>\r\n");
            data.Append("\t\tpublic " + param.ClassName + "DAL()\r\n");
            data.Append("\t\t: base(\"" + param.ClassName + "\", \"ID\")\r\n");
            data.Append("\t\t{\r\n");
            data.Append("\t\t\tthis.sortField = \"ID\";\r\n");
            data.Append("\t\t\tthis.IsDescending = false;\r\n");
            data.Append("\t\t}\r\n");


            //新增记录


            data.Append("\t}\r\n");
            data.Append("}");
            return data.ToString();*/
            string filePath = System.IO.Directory.GetCurrentDirectory() + "\\Template\\DAL.txt";
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

            string result = Engine.Razor.RunCompile(value, Guid.NewGuid().ToString(), null, new { param = param });

            return result.ToString();
        }


        /// <summary>
        /// 新增记录方法
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="param"></param>
        /// <returns></returns>


    }
}
