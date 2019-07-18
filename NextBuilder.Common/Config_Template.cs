using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;

namespace NextBuilder.Common
{
    public class Config_Template
    {
        public Config_Template()
        {
            XmlFileExists();
        }
        /// <summary>
        /// XML文件路径
        /// </summary>
        private string XmlFile = string.Format("{0}Config\\Template.xml", Func.GetAppPath());
        /// <summary>
        /// 检查配置文件是否存在，没有则创建
        /// </summary>
        private void XmlFileExists()
        {
            FileInfo fiXML = new FileInfo(XmlFile);
            if (!(fiXML.Exists))
            {
                XDocument xelLog = new XDocument(
                    new XDeclaration("1.0", "utf-8", string.Empty),
                    new XElement("root")
                 );
                xelLog.Save(XmlFile);
            }
        }
        /// <summary>
        /// 得到所有类命名空间
        /// </summary>
        /// <returns></returns>
        public List<Model.ConfigTemplate> GetAll()
        {
            List<Model.ConfigTemplate> list = new List<Model.ConfigTemplate>();
            try
            {
                XElement xelem = XElement.Load(XmlFile);
                var queryXML = from xele in xelem.Elements("Template")
                               select new
                               {
                                   templateName = xele.Element("TemplateName").Value,
                                   directoryName = xele.Element("DirectoryName").Value,
                                   targetName = xele.Element("TargetName").Value,
                                   targetPostfix = xele.Element("TargetPostfix").Value,
                                   isActive = xele.Element("IsActive").Value

                               };
                foreach (var q in queryXML)
                {
                    list.Add(new Model.ConfigTemplate()
                    {
                        TemplateName = q.templateName,
                        DirectoryName = q.directoryName,
                        TargetName = q.targetName,
                        TargetPostfix = q.targetPostfix,
                        IsActive = q.isActive

                    });
                }
                
            }
            catch (Exception e) 
            {
                e.ToString(); 
            }
            return list;
        }
        /// <summary>
        /// 添加一个类命名空间
        /// </summary>
        /// <param name="cns"></param>
        public bool Add(Model.ConfigTemplate cns)
        {
            try
            {
                //先删除
                Delete(cns.TemplateName);
                XElement xelem = XElement.Load(XmlFile);
                XElement newLog = new XElement("Template",
                                      new XElement("TemplateName", cns.TemplateName),
                                      new XElement("DirectoryName", cns.DirectoryName),
                                      new XElement("TargetName", cns.TargetName),
                                      new XElement("TargetPostfix", cns.TargetPostfix),
                                      new XElement("IsActive", cns.IsActive)
                                  );
                xelem.Add(newLog);
                xelem.Save(XmlFile);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 保存类命名空间
        /// </summary>
        /// <param name="cns"></param>
        /// <returns></returns>
        public bool Save(Model.ConfigTemplate cns, string oldmodel = "")
        {
            if (!oldmodel.IsNullOrEmpty())
                Delete(oldmodel);
            return Add(cns);
        }
        /// <summary>
        /// 删除一个命名空间
        /// </summary>
        /// <param name="namespace1"></param>
        /// <returns></returns>
        public bool Delete(string model)
        {
            try
            {
                XElement xelem = XElement.Load(XmlFile);
                var queryXML = from xele in xelem.Elements("Template")
                               where xele.Element("TemplateName").Value == model
                               select xele;
                queryXML.Remove();
                xelem.Save(XmlFile);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 查询一个命名空间
        /// </summary>
        /// <param name="namespace1"></param>
        /// <returns></returns>
        public Model.ConfigTemplate Get(string model)
        {
            try
            {
                XElement xelem = XElement.Load(XmlFile);
                var queryXML = from xele in xelem.Elements("Template")
                               where xele.Element("TemplateName").Value == model
                               select new
                               {
                                   templateName = xele.Element("TemplateName").Value,
                                   directoryName = xele.Element("DirectoryName").Value,
                                   targetName = xele.Element("TargetName").Value,
                                   targetPostfix = xele.Element("TargetPostfix").Value,
                                   isActive = xele.Element("IsActive").Value.ToLower()
                               };
                Model.ConfigTemplate cns = new Model.ConfigTemplate();
                if (queryXML.Count() > 0)
                {
                    cns.TemplateName = queryXML.First().templateName;
                    cns.DirectoryName = queryXML.First().directoryName;
                    cns.TargetName = queryXML.First().targetName;
                    cns.TargetPostfix = queryXML.First().targetPostfix;
                    cns.IsActive = queryXML.First().isActive;
                }
                return cns;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 查询默认命名空间
        /// </summary>
        /// <param name="namespace1"></param>
        /// <returns></returns>
        public Model.ConfigTemplate GetDefault()
        {
            var list = GetAll();
            if (list.Count == 0)
            {
                return new Model.ConfigTemplate()
                {
                    TemplateName = "",
                    DirectoryName = "",
                    TargetName = "",
                    TargetPostfix = "",
                    IsActive = "False"
                };
            }
            else
            {
                return list.Last();
            }

        }
    }
}
