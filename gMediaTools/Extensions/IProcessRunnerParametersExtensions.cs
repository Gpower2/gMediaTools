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
                    // Check if the parameter must be included
                    if (!parameter.Include)
                    {
                        // Parameter should not be included, skip it
                        Debug.WriteLine($"Found {parameter.Name} which should not be included, skipping...");

                        continue;
                    }

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

        /// <summary>
        /// Search for a parameter by its name and set it to Include = true and set the value
        /// Returns the IProcessRunnerParameters object to enable chaining
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parameterName"></param>
        public static IProcessRunnerParameters IncludeParameterWithValue(this IProcessRunnerParameters parameters, string parameterName, string parameterValue)
        {
            // Search all groups
            foreach (var group in parameters.ParameterGroups)
            {
                // Search all group parameters
                foreach (var parameter in group.Parameters)
                {
                    // Check for parameter name
                    if (parameter.Name == parameterName)
                    {
                        // Parameter found!

                        // Set parameter to Include = true and set the Value
                        parameter.Include = true;
                        parameter.Value = parameterValue;

                        return parameters;
                    }
                }
            }

            // Parameter was not found!
            throw new Exception($"Parameter {parameterName} was not found!");
        }

        /// <summary>
        /// Search for a parameter by its name and set it to Include = true
        /// Returns the IProcessRunnerParameters object to enable chaining
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="parameterName"></param>
        public static IProcessRunnerParameters IncludeParameterWithNoValue(this IProcessRunnerParameters parameters, string parameterName)
        {
            // Search all groups
            foreach (var group in parameters.ParameterGroups)
            {
                // Search all group parameters
                foreach (var parameter in group.Parameters)
                {
                    // Check for parameter name
                    if (parameter.Name == parameterName)
                    {
                        // Parameter found!

                        // Check if parameter allows for empty values
                        if (!parameter.AllowsEmptyValues)
                        {
                            throw new Exception($"Parameter {parameterName} was found, but it doesn't allow empty values!");
                        }

                        // Set parameter to Include = true
                        parameter.Include = true;
                        parameter.Value = "";

                        return parameters;
                    }
                }
            }

            // Parameter was not found!
            throw new Exception($"Parameter {parameterName} was not found!");
        }

        /// <summary>
        /// Set all parameters to Include = false and Value = ""
        /// Returns the IProcessRunnerParameters object to enable chaining
        /// </summary>
        /// <param name="parameters"></param>
        public static IProcessRunnerParameters ResetParameters(this IProcessRunnerParameters parameters)
        {
            foreach (var group in parameters.ParameterGroups)
            {
                foreach (var parameter in group.Parameters)
                {
                    parameter.Include = false;
                    parameter.Value = "";
                }
            }

            return parameters;
        }
    }
}
