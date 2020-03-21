using gMediaTools.Extensions;
using gMediaTools.Models.ProcessRunner;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gMediaTools.Services.ProcessRunner
{
    public abstract class BaseProcessRunnerService : IProcessRunnerService
    {
        protected const string ProcessFileName = "";

        public int RunProcess(IProcessRunnerParameters parameters, Action<Process, string> lineAction)
        {
            // Create the ProcessStartInfo object
            ProcessStartInfo myProcessInfo = new ProcessStartInfo
            {
                FileName = ProcessFileName,
                Arguments = parameters.GetProcessParametersString(),

                // ====================================================
                UseShellExecute = false,
                RedirectStandardOutput = true,
                StandardOutputEncoding = Encoding.UTF8,
                RedirectStandardError = true,
                StandardErrorEncoding = Encoding.UTF8,
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden
                // ====================================================
            };

            using (Process myProcess = new Process())
            {
                myProcess.StartInfo = myProcessInfo;

                Debug.WriteLine(myProcessInfo.Arguments);

                // Start the process
                myProcess.Start();

                // Read the Standard output character by character
                myProcess.ReadStreamPerCharacter(lineAction);
                
                // Wait for the process to exit
                myProcess.WaitForExit();

                // Debug write the exit code
                Debug.WriteLine($"Exit code: {myProcess.ExitCode}");

                // Return the process exit code
                return myProcess.ExitCode;
            }
        }
    }
}
