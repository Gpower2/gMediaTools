using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public interface IProcessRunnerParameter
    {
        string Name { get; set; }

        string Value { get; set; }

        string NamePrefix { get; set; }

        string NameValueSeparator { get; set; }

        bool ValueNeedsToBeQuoted { get; set; }

        bool AllowsEmptyValues { get; set; }

        Func<string, string> ProcessValue { get; set; }
    }
}
