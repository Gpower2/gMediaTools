using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public class DefaultProcessRunnerParameters : IProcessRunnerParameters
    {
        public IList<IProcessRunnerParameterGroup> ParameterGroups { get; } = new List<IProcessRunnerParameterGroup>();
        public string ParameterGroupSeparator { get; }

        public string ProcessFileName { get; }

        public bool UseOutputStream { get; }


        public DefaultProcessRunnerParameters(string processFileName, string parameterGroupSeparator, bool useOutputStream = true)
        {
            ProcessFileName = processFileName ?? throw new ArgumentNullException(nameof(processFileName));
            ParameterGroupSeparator = parameterGroupSeparator ?? throw new ArgumentNullException(nameof(parameterGroupSeparator));
            UseOutputStream = useOutputStream;
        }
    }
}
