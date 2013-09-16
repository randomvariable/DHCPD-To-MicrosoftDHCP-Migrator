using Irony.Interpreter.Ast;
using System.Xml;

namespace DhcpdToMicrosoft.Compiler.Config
{
    class ConfFile : AstNode
    {
        public XmlDocument xml {get; private set;}


        public ConfFile() : base()
        {
            XmlDocument xml = new XmlDocument();

        }
        
    }
}
