using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public interface IProcessRunnerParameter
    {
        string Name { get; }

        string Value { get; set; }

        string NamePrefix { get; }

        string NameValueSeparator { get; }

        bool ValueNeedsToBeQuoted { get; }

        bool AllowsEmptyValues { get; }

        bool Include { get; set; }

        Func<string, string> ProcessValue { get; set; }
    }
}
