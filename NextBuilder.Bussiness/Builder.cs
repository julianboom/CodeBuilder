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
    internal class Builder
    {
        private IData.ICreateCode createInstance;
        private IData.IDatabase databaseInstance;
        private Import import;
        public Builder(Model.DatabaseType dbType)
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
        public string GetBuilderClass(Model.CodeCreate param,string templateName)
        {
            Model.Servers server = Common.Config.GetServer(param.ServerID);
            if (server == null)
            {
                return string.Empty;
            }
            List<Model.Fields> fields = databaseInstance.GetFields(server.ID, param.DbName, param.TableName);

            string filePath = System.IO.Directory.GetCurrentDirectory() + "\\Template\\"+templateName;//"DAL.txt";
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

            string result = Engine.Razor.RunCompile(value, Guid.NewGuid().ToString(), null, new { param = param, fields = fields });
            result = result.Replace("[*]", "@");

            return result.ToString();
        }

    }
}
