using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using System.IO;
using DhcpdToMicrosoft.Parser;
using Antlr4.Runtime;
using DhcpdToMicrosoft.Compiler;
using NLog;

namespace DhcpdToMicrosoftCLI
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string text = System.IO.File.ReadAllText(@args[0]);
            var stream = new AntlrInputStream(text);
            var lexer = new DHCPDConfigLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new DHCPDConfigParser(tokens);
            var tree = parser.config();
            var compiler = new DhcpdToMicrosoftCompiler();
            compiler.Visit(tree);
            StreamWriter file = new StreamWriter(@args[1], false, Encoding.Unicode);
            string xml = compiler.CompileXML();
            file.WriteLine(xml);
            file.Close();
        }
    }
}
