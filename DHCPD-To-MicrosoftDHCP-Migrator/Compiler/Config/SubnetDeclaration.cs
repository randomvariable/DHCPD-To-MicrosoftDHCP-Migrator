using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DhcpdToMicrosoft.Model;
using Irony.Interpreter.Ast;

namespace DHCPD_To_MicrosoftDHCP_Migrator.Compiler.Config
{
    class SubnetDeclaration : AstNode
    {
        SubnetDeclaration()
        {
            ScopeIPv4 scope = new ScopeIPv4();
            
        }
    }
}
