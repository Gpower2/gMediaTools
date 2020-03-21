using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Models.ProcessRunner
{
    public class QuotedValueProcessRunnerParameter : IProcessRunnerParameter
    {
        public string Name { get; }
        public string Value { get; set; }
        public string NamePrefix { get; }
        public string NameValueSeparator { get; }
        public bool ValueNeedsToBeQuoted { get; } = true;
        public bool AllowsEmptyValues { get; } = false;

        public Func<string, string> ProcessValue { get; set; }

        public QuotedValueProcessRunnerParameter(string name, string namePrefix, string nameValueSeparator, Func<string, string> processValue = null)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            NamePrefix = namePrefix ?? throw new ArgumentNullException(nameof(namePrefix));
            NameValueSeparator = nameValueSeparator ?? throw new ArgumentNullException(nameof(nameValueSeparator));
            ProcessValue = processValue ?? new Func<string, string>((string value) => { return value; });
        }
    }
}
