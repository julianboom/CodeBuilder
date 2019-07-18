using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBuilder.Model
{
    public class ConfigTemplate
    {
        public string TemplateName { get; set; }

        public string DirectoryName { get; set; }

        public string TargetName { get; set; }

        public string TargetPostfix { get; set; }

        public string IsActive { get; set; }
    }
}
