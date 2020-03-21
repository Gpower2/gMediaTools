using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public class DefaultProcessRunnerParameterGroup : IProcessRunnerParameterGroup
    {
        public string Name { get; }
        public int Order { get; set; }
        public string ParameterSeparator { get; }
        public IList<IProcessRunnerParameter> Parameters { get; } = new List<IProcessRunnerParameter>();

        public DefaultProcessRunnerParameterGroup(string name, int order, string parameterSeparator)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Order = order;
            ParameterSeparator = parameterSeparator ?? throw new ArgumentNullException(nameof(parameterSeparator));
        }
    }
}
