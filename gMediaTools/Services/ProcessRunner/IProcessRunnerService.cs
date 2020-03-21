using gMediaTools.Models.ProcessRunner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.ProcessRunner
{
    public interface IProcessRunnerService
    {
        int RunProcess(IProcessRunnerParameters parameters, Action<Process, string> lineAction);
    }
}
