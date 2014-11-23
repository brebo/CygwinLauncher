using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;

namespace Brebo.CygwinLauncher.Launcher
{
    public class CommandLineOptions
    {
        [Option('d', "directory", Required = false, MutuallyExclusiveSet = "file")]
        public string Directory { get; set; }

        [Option('c', "command", Required = false, MutuallyExclusiveSet = "file")]
        public string Command { get; set; }
    }
}
