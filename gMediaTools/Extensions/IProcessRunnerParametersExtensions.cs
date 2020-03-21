using gMediaTools.Models.ProcessRunner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Extensions
{
    public static class IProcessRunnerParametersExtensions
    {
        public static string GetProcessParametersString(this IProcessRunnerParameters parameters)
        {
            // Create a list containing all the group parameter strings
            List<string> finalParameters = new List<string>();

            // Get the parameters for each group in the group's order
            foreach (var parameterGroup in parameters.ParameterGroups.OrderBy(g => g.Order))
            {
                // Create a list containing all the parameters for the specific group
                List<string> groupParameters = new List<string>();

                // Get the parameters for the specific group
                foreach (var parameter in parameterGroup.Parameters)
                {
                    StringBuilder sb = new StringBuilder();

                    sb.Append(parameter.NamePrefix);
                    sb.Append(parameter.Name);

                    // Process the value
                    string processedValue = parameter.ProcessValue(parameter.Value);

                    // Check for empty value
                    if (string.IsNullOrWhiteSpace(processedValue))
                    {
                        // Found empty value

                        // Check if parameter allows empty values
                        if (!parameter.AllowsEmptyValues)
                        {
                            // No empty values allowed, skip the parameter
                            Debug.WriteLine($"Found {parameter.Name} with empty value, skipping...");

                            continue;
                        }
                    }
                    else
                    {
                        // No empty value found
                        sb.Append(parameter.NameValueSeparator);

                        // Check if the parameter needs to be quoted
                        if (parameter.ValueNeedsToBeQuoted)
                        {
                            sb.Append($"\"{processedValue}\"");
                        }
                        else
                        {
                            sb.Append(processedValue);
                        }
                    }

                    // Add the parameter string in the group parameters list
                    groupParameters.Add(sb.ToString());
                }

                // Add the group parameter string in the final list
                finalParameters.Add(string.Join(parameterGroup.ParameterSeparator, groupParameters));
            }

            return string.Join(parameters.ParameterGroupSeparator, finalParameters);
        }
    }
}
