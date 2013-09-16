using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DhcpdToMicrosoft.Parser;
using Irony.Parsing;
using System.IO;


namespace DhcpdToMicrosoftCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigGrammar grammar = new ConfigGrammar();
            Parser parser = new Parser(grammar);
            using (StreamReader sr = new StreamReader(@"I:\Work\DHCPD-To-MicrosoftDHCP-Migrator\ucltestdata\dhcpd-new.conf"))
            {
                String text = sr.ReadToEnd();
                parser.Parse(text);
               parser
                   
                   Console.WriteLine(parser.Root.Name.ToString());
                Console.Read();
            }
    
        }
    }
}
