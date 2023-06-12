using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Common.Processors;
using Intent.Utils;

namespace Intent.Modules.Angular
{
    public static class CliCommand
    {
        public static void Run(string location, string command)
        {
            if (!Directory.Exists(Path.GetFullPath(location)))
            {
                Directory.CreateDirectory(Path.GetFullPath(location));
            }
            try
            {
                Logging.Log.Info($"Executing: {location}> {command}");
                var output = ExecuteCommand(Path.GetFullPath(location),
                    new[]
                    {
                        command,
                    });
                Logging.Log.Info(output);
            }
            catch (Exception e)
            {
                Logging.Log.Failure($@"Failed to execute: ""{command}""
Please ensure that both npm and that the Angular CLI have been installed.

To check that you have the npm client installed, run the following command in a terminal/console window:
npm -v

To install the CLI using npm, open a terminal/console window and enter the following command:
npm install -g @angular/cli@16");
                Logging.Log.Failure(e);
            }

        }

        /// <summary>
        /// Copied and adapted from <see cref="CommandLineProcessor.ExecuteCommand"/>
        /// </summary>
        private static string ExecuteCommand(string workingDirectory, params string[] commands)
        {
            var process = new Process()
            {
                StartInfo = {
                    FileName = "cmd.exe",
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    WorkingDirectory = workingDirectory,
                }
            };

            process.StartInfo.EnvironmentVariables.Add("NG_CLI_ANALYTICS", "false");

            process.Start();

            foreach (string command in commands)
            {
                process.StandardInput.WriteLine(command);
            }

            process.StandardInput.Flush();
            process.StandardInput.Close();
            return process.StandardOutput.ReadToEnd();
        }

        public static IOutputTarget GetFrontEndOutputTarget(IApplication application)
        {
            return application.OutputTargets.FirstOrDefault(x => x.HasRole("Front End"));
        }
    }
}