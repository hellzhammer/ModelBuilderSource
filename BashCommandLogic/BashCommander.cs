using System;
using System.Diagnostics;
using Gtk;
using System.Threading.Tasks;
namespace TestApp.BashCommandLogic
{
    // wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
    // sudo dpkg -i packages-microsoft-prod.deb
    public class BashCommander
    {
        public Process modelBuildProc { get; set; }

        public void ExecuteBashCommand(string command)
        {
            command = command.Replace("\"", "\"\"");

            modelBuildProc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            modelBuildProc.Start();
        }

        public string ModelBuildCommandBuilder(string datapath, string labelcolumn, string ignorecolumns, bool hasheader, double maxexplorationtime)
        {
            string mlTask = "mlnet auto-train --task multiclass-classification ";

            string mlData = "--dataset " + datapath + " ";

            string labelColumn = "--label-column-name " + labelcolumn + " ";

            string ignoreColumns = "";
            if (!string.IsNullOrEmpty(ignorecolumns))
            {
                ignoreColumns = "--ignore-columns " + ignorecolumns + " ";
            }

            string hasHeader = "--has-header " + hasheader.ToString() + " ";

            string maxExplorationTime = "--max-exploration-time " + int.Parse(maxexplorationtime.ToString()).ToString();
            return mlTask + mlData + labelColumn + ignoreColumns + hasHeader + maxExplorationTime;
        }
    }
}
