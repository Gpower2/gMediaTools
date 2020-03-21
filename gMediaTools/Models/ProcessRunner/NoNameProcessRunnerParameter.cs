using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public class NoNameProcessRunnerParameter : IProcessRunnerParameter
    {
        public string Name { get; }
        public string Value { get; set; }
        public string NamePrefix { get; }
        public string NameValueSeparator { get; }
        public bool ValueNeedsToBeQuoted { get; }
        public bool AllowsEmptyValues { get; } = false;
        public bool ValueOnlyOutput { get; } = true;
        public bool Include { get; set; } = false;

        public Func<string, string> ProcessValue { get; set; }

        public NoNameProcessRunnerParameter(string name, bool valueNeedsToBeQuoted, Func<string, string> processValue = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ValueNeedsToBeQuoted = valueNeedsToBeQuoted;
            ProcessValue = processValue ?? new Func<string, string>((string value) => { return value; });
        }
    }
}
