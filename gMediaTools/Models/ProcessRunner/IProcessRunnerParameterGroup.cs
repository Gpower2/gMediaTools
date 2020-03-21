using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public interface IProcessRunnerParameterGroup
    {        
        string Name { get; }

        int Order { get; set; }

        string ParameterSeparator { get; }

        IEnumerable<IProcessRunnerParameter> Parameters { get; }
    }
}
