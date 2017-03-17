using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace CacheInitializer
{
    // Define a class to receive parsed values
    class Options
    {
   
        [Option('s', "server", Required = true,
          HelpText = "URL to the server.")]
        public string server { get; set; }

        [Option('a', "appname", Required = false,
          HelpText = "App to load")]
        public string appname { get; set; }

        [Option('p', "appname", Required = false,
          HelpText = "Virtual Proxy to use")]
        public string virtualProxy { get; set; }

        [Option('i', "index", Required = false, DefaultValue = false,
            HelpText = "creates the smart search index")]
        public bool createsearchindex { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
