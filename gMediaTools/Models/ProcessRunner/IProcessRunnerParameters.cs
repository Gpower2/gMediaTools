using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public interface IProcessRunnerParameters
    {
        IEnumerable<IProcessRunnerParameterGroup> ParameterGroups { get; }

        string ParameterGroupSeparator { get; }

        string ProcessFileName { get; }
    }
}
