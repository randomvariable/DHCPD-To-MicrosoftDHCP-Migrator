using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DhcpdToMicrosoft.Parser;

namespace DhcpdToMicrosoft.Service
{
    public class DhcpdParser
    {
        static readonly ConfigGrammar Grammar = new ConfigGrammar();

        public static void ConfigParse(string configFile)
        {
           // var compiler = new LanguageCompiler(ConfigGrammar);
            //return compiler.Parse(configFile).ToString();
        }
        
        
    }
}