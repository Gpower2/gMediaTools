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
    public class DefaultProcessRunnerService : IProcessRunnerService, IDisposable
    {
        private Process _process;

        public int RunProcess(IProcessRunnerParameters parameters, Action<Process, string> lineAction)
        {
            // Create the ProcessStartInfo object
            ProcessStartInfo myProcessInfo = new ProcessStartInfo
            {
                FileName = parameters.ProcessFileName,
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

            using (_process = new Process())
            {
                _process.StartInfo = myProcessInfo;

                Debug.WriteLine(myProcessInfo.Arguments);

                // Start the process
                _process.Start();

                // Read the Standard output character by character
                Task.Run(() => _process.ReadStreamPerCharacter(parameters.UseOutputStream, lineAction));

                // Wait for the process to exit
                _process.WaitForExit();

                // Debug write the exit code
                Debug.WriteLine($"Exit code: {_process.ExitCode}");

                // Return the process exit code
                return _process.ExitCode;
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    try
                    {
                        if (_process!= null && !_process.HasExited)
                        {
                            _process.Kill();

                            _process.WaitForExit();
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }

                    _process = null;
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }

        #endregion
    }
}
