using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DancingDuck.Util
{
    class Options
    {
		[Option('u', "url", Required=true, 
            HelpText="The url to start crawling from")]
        public string InputUrl { get; set; }

        [Option('v', "variant", Required = false, DefaultValue = "compmanager",
            HelpText = "Currently unused, determines the type of crawler to employ")]
        public string Variant { get; set; }

        [Option('o', "output", Required = true, HelpText = "The file for results")]
        public string Output { get; set; }

		[ParserState]
        public IParserState LastParserState { get; set; }

		[HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }
}
