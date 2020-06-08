using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBConfigTool
{
    class Options
    {
        [Option('i',"input",Required =true,HelpText ="")]
        public string InputFilePath { get; set; }
        [Option('o',"output",Required =true,HelpText ="")]
        public string OutputDirPath { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptions);
        }

        static void RunOptions(Options options)
        {

        }
    }
}
