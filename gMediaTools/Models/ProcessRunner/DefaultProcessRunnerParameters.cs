using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public class DefaultProcessRunnerParameters : IProcessRunnerParameters
    {
        public IEnumerable<IProcessRunnerParameterGroup> ParameterGroups { get; } = new List<IProcessRunnerParameterGroup>();
        public string ParameterGroupSeparator { get; }

        public DefaultProcessRunnerParameters(string parameterGroupSeparator)
        {
            ParameterGroupSeparator = parameterGroupSeparator ?? throw new ArgumentNullException(nameof(parameterGroupSeparator));
        }
    }
}
