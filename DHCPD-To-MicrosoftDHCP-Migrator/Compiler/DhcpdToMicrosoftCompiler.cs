
namespace DhcpdToMicrosoft.Compiler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DhcpdToMicrosoft.Parser;
    using Antlr4.Runtime.Tree;
    using IToken = Antlr4.Runtime.IToken;
    using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;
    using Antlr4.Runtime.Misc;
    using DhcpdToMicrosoft.Model;
    using System.Xml.Serialization;

    class DhcpdToMicrosoftCompiler : AbstractParseTreeVisitor<Object>, IDHCPDConfigVisitor<Object>
    {
        public Object VisitRangeLow6([NotNull] DHCPDConfigParser.RangeLow6Context context) { return VisitChildren(context); }
        public Object VisitNetmask([NotNull] DHCPDConfigParser.NetmaskContext context) { return VisitChildren(context); }
        public Object VisitSubnet6Declaration([NotNull] DHCPDConfigParser.Subnet6DeclarationContext context) { return VisitChildren(context); }
        public Object VisitSharedNetwork([NotNull] DHCPDConfigParser.SharedNetworkContext context) { return VisitChildren(context); }
        public Object VisitConfig([NotNull] DHCPDConfigParser.ConfigContext context)
        {
            NewDataSet dataset = new NewDataSet();
            List<XmlElementAttribute> itemList = new List<XmlElementAttribute>();
            itemList.Add(VisitChildren(context) as XmlElementAttribute);
            return dataset.ToString();           
        }

  
        public Object VisitRangeHigh([NotNull] DHCPDConfigParser.RangeHighContext context) { return VisitChildren(context); }
        public Object VisitDeclaration([NotNull] DHCPDConfigParser.DeclarationContext context) { return VisitChildren(context); }
        public Object VisitFixedPrefix6([NotNull] DHCPDConfigParser.FixedPrefix6Context context) { return VisitChildren(context); }
        public Object VisitHostname([NotNull] DHCPDConfigParser.HostnameContext context) { return VisitChildren(context); }
        public Object VisitAddressRangeDeclaration([NotNull] DHCPDConfigParser.AddressRangeDeclarationContext context) { return VisitChildren(context); }
        public Object VisitAddressRange6Declaration([NotNull] DHCPDConfigParser.AddressRange6DeclarationContext context) { return VisitChildren(context); }
        public Object VisitLeaseAddress([NotNull] DHCPDConfigParser.LeaseAddressContext context) { return VisitChildren(context); }
        public Object VisitRangeLow([NotNull] DHCPDConfigParser.RangeLowContext context) { return VisitChildren(context); }
        public Object VisitIp4Address([NotNull] DHCPDConfigParser.Ip4AddressContext context) { return VisitChildren(context); }
        public Object VisitStatement([NotNull] DHCPDConfigParser.StatementContext context) { return VisitChildren(context); }
        public Object VisitLease([NotNull] DHCPDConfigParser.LeaseContext context) { return VisitChildren(context); }
        public Object VisitOptionOptionStatement([NotNull] DHCPDConfigParser.OptionOptionStatementContext context) { return VisitChildren(context); }
        public Object VisitOptionParam([NotNull] DHCPDConfigParser.OptionParamContext context) { return VisitChildren(context); }
        public Object VisitIpAddressWithSubnet([NotNull] DHCPDConfigParser.IpAddressWithSubnetContext context) { return VisitChildren(context); }
        public Object VisitFixedAddress([NotNull] DHCPDConfigParser.FixedAddressContext context) { return VisitChildren(context); }
        public Object VisitLeaseParameters([NotNull] DHCPDConfigParser.LeaseParametersContext context) { return VisitChildren(context); }
        public Object VisitSharedNetworkDeclaration([NotNull] DHCPDConfigParser.SharedNetworkDeclarationContext context) { return VisitChildren(context); }
        public Object VisitOptionStatement([NotNull] DHCPDConfigParser.OptionStatementContext context) { return VisitChildren(context); }
        public Object VisitSubnetDeclaration([NotNull] DHCPDConfigParser.SubnetDeclarationContext context) { return VisitChildren(context); }
        public Object VisitLeaseTime([NotNull] DHCPDConfigParser.LeaseTimeContext context){
            return VisitChildren(context);
        }
        public Object VisitFailoverDeclaration([NotNull] DHCPDConfigParser.FailoverDeclarationContext context) { return VisitChildren(context); }
        public Object VisitKlass([NotNull] DHCPDConfigParser.KlassContext context) { return VisitChildren(context); }
        public Object VisitState([NotNull] DHCPDConfigParser.StateContext context) { return VisitChildren(context); }
        public Object VisitGroupDeclaration([NotNull] DHCPDConfigParser.GroupDeclarationContext context) { return VisitChildren(context); }
        public Object VisitDate([NotNull] DHCPDConfigParser.DateContext context) { return VisitChildren(context); }
        public Object VisitTimestamp([NotNull] DHCPDConfigParser.TimestampContext context) { return VisitChildren(context); }
        public Object VisitHostDeclaration([NotNull] DHCPDConfigParser.HostDeclarationContext context) { return VisitChildren(context); }
        public Object VisitSubnet([NotNull] DHCPDConfigParser.SubnetContext context) { return VisitChildren(context); }
        public Object VisitParameter([NotNull] DHCPDConfigParser.ParameterContext context) { return VisitChildren(context); }
        public Object VisitLeaseDeclaration([NotNull] DHCPDConfigParser.LeaseDeclarationContext context) { return VisitChildren(context); }
        public Object VisitFailoverStateStatement([NotNull] DHCPDConfigParser.FailoverStateStatementContext context) { return VisitChildren(context); }
        public Object VisitStatements([NotNull] DHCPDConfigParser.StatementsContext context) { return VisitChildren(context); }
        public Object VisitFixedAddressParameter([NotNull] DHCPDConfigParser.FixedAddressParameterContext context) { return VisitChildren(context); }
        public Object VisitSubnet6([NotNull] DHCPDConfigParser.Subnet6Context context) { return VisitChildren(context); }
        public Object VisitHostnameOrIpAddress([NotNull] DHCPDConfigParser.HostnameOrIpAddressContext context) { return VisitChildren(context); }
        public Object VisitIp6net([NotNull] DHCPDConfigParser.Ip6netContext context) { return VisitChildren(context); }
        public Object VisitIp6Address([NotNull] DHCPDConfigParser.Ip6AddressContext context) { return VisitChildren(context); }
        public Object VisitHardwareParameter([NotNull] DHCPDConfigParser.HardwareParameterContext context) { return VisitChildren(context); }
        public Object VisitStringParameter([NotNull] DHCPDConfigParser.StringParameterContext context) { return VisitChildren(context); }
        public Object VisitClassDeclaration([NotNull] DHCPDConfigParser.ClassDeclarationContext context) { return VisitChildren(context); }
        public Object VisitRangeHigh6([NotNull] DHCPDConfigParser.RangeHigh6Context context) { return VisitChildren(context); }
        public Object VisitIpAddrOrHostnames([NotNull] DHCPDConfigParser.IpAddrOrHostnamesContext context) { return VisitChildren(context); }
        public Object VisitStartEnd([NotNull] DHCPDConfigParser.StartEndContext context) { return VisitChildren(context); }
        public Object VisitLeaseParameter([NotNull] DHCPDConfigParser.LeaseParameterContext context) { return VisitChildren(context); }
        public Object VisitPoolDeclaration([NotNull] DHCPDConfigParser.PoolDeclarationContext context) { return VisitChildren(context); }
        public Object VisitIp6Prefix([NotNull] DHCPDConfigParser.Ip6PrefixContext context) { return VisitChildren(context); }
        public Object VisitPeerStatement([NotNull] DHCPDConfigParser.PeerStatementContext context) { return VisitChildren(context); }
        public Object VisitFailoverStateDeclaration([NotNull] DHCPDConfigParser.FailoverStateDeclarationContext context) { return VisitChildren(context); }
    }
}
