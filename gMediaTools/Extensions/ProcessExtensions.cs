using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Extensions
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Reads a Process's standard output stream character by character and calls the user defined Action for each line
        /// </summary>
        /// <param name="process"></param>
        /// <param name="lineAction"></param>
        public static void ReadStreamPerCharacter(this Process process, Action<Process, string> lineAction)
        {
            // Store the process StandardOuput in a user friendly variable
            StreamReader outputReader = process.StandardOutput;

            // Define a line builder to store the line data
            StringBuilder lineBuilder = new StringBuilder();

            // Start reading the stream
            while (true)
            {
                // Check for end of stream
                if (!outputReader.EndOfStream)
                {
                    // Consume the next character from the stream
                    char c = (char)outputReader.Read();

                    // Check for possible line ending character
                    if (c == '\r')
                    {
                        // Possible line ending

                        // Check for line ending by peeking (not consuming) the next character
                        if ((char)outputReader.Peek() == '\n')
                        {
                            // End of line, consume the next character
                            outputReader.Read();
                        }

                        // Call the user defined line Action
                        lineAction?.Invoke(process, lineBuilder.ToString());

                        // Reset the StringBuilder
                        lineBuilder.Length = 0;
                    }
                    else if (c == '\n')
                    {
                        // Definite line ending

                        // End of line, call the user defined line Action
                        lineAction?.Invoke(process, lineBuilder.ToString());

                        // Reset the StringBuilder
                        lineBuilder.Length = 0;
                    }
                    else
                    {
                        // No line ending, append the character to the line data
                        lineBuilder.Append(c);
                    }
                }
                else
                {
                    // Reached the end of stream, break the loop
                    break;
                }
            }
        }
    }
}
