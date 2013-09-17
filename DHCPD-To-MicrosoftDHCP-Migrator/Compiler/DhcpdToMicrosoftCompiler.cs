
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
    using DhcpdToMicrosoft.Utility;
    using System.Net;
    using NLog;

    public class DhcpdToMicrosoftCompiler : AbstractParseTreeVisitor<Object>, IDHCPDConfigVisitor<Object>
    {
        private List<IPv4Lease> Leases;
        private List<IPv4Filter> Filters;
        private List<IPv4Reservation> Reservations;
        private List<ScopeIPv4> Scopesv4;
        private List<IPRange> IpRanges;
        private ParseTreeProperty<IPAddress> IPAddresses;
        private ParseTreeProperty<string> BindingStates;
        private ParseTreeProperty<string> ClassNames;
        private ParseTreeProperty<string> ClassTypes;
        private ParseTreeProperty<string> ClassData;
        private ParseTreeProperty<string> ClassDescriptions;
        private ParseTreeProperty<int> ClassVendorID;
        private ParseTreeProperty<string> OptionDefinitionNames;
        private ParseTreeProperty<int> OptionIDs;
        private ParseTreeProperty<string> OptionTypes;
        private ParseTreeProperty<bool> OptionMultivalued;
        private ParseTreeProperty<string> OptionDescriptions;
        private ParseTreeProperty<string> OptionVendorClasses;
        private ParseTreeProperty<string> OptionDefaultValues;
        private ParseTreeProperty<string> PolicyNames;
        private ParseTreeProperty<int> PolicyProcessingOrders;
        private ParseTreeProperty<bool> PolicyEnabled;
        private ParseTreeProperty<string> PolicyConditions;
        private ParseTreeProperty<string> PolicyDescriptions;
        private ParseTreeProperty<string> PolicyVendorClasses;
        private ParseTreeProperty<string> PolicyVendorClassColumns;
        private ParseTreeProperty<string> PolicyUserClassColumns;
        private ParseTreeProperty<string> PolicyMacAddressColumns;
        private ParseTreeProperty<string> PolicyClientIdColumns;
        private ParseTreeProperty<string> PolicyRelayAgentColumns;
        private ParseTreeProperty<string> PolicyCircuitIdColumns;
        private ParseTreeProperty<string> PolicyRemoteIds;
        private ParseTreeProperty<string> PolicySubscriberIds;
        private ParseTreeProperty<IPAddress> IpRangeStartRanges;
        private ParseTreeProperty<IPAddress> IpRangeEndRanges;
        private ParseTreeProperty<int> OptValueOptionIds;
        private ParseTreeProperty<string> OptValueOptionVendorClassStrings;
        private ParseTreeProperty<string> OptValueOptionUserClassStrings;
        private ParseTreeProperty<string> OptValueOptionValues;
        private ParseTreeProperty<string> OptValueOptionValueColumns;
        private ParseTreeProperty<IPAddress> ScopeIPv4ScopeIDs;
        private ParseTreeProperty<string> ScopeIPv4Names;
        private ParseTreeProperty<string> ScopeIPv4SubnetMasks;
        private ParseTreeProperty<string> ScopeIPv4StartRanges;
        private ParseTreeProperty<string> ScopeIPv4EndRanges;
        private ParseTreeProperty<string> ScopeIPv4LeaseDurations;
        private ParseTreeProperty<string> ScopeIPv4States;
        private ParseTreeProperty<string> ScopeIPv4Types;
        private ParseTreeProperty<int> ScopeIPv4MaxBootpClients;
        private ParseTreeProperty<bool> ScopeIPv4NapEnables;
        private ParseTreeProperty<string> ScopeIPv4Descriptions;
        private ParseTreeProperty<bool> ScopeIPv4ActivatePolicies;
        private ParseTreeProperty<string> ScopeIPv4SuperscopeNames;
        private ParseTreeProperty<IPRange[]> ScopeIPv4ExclusionRanges;
        private ParseTreeProperty<Policies> ScopeIPv4Policies;
        private ParseTreeProperty<OptionValues> ScopeIPv4OptionValues;
        private ParseTreeProperty<IPv4Reservation[]> ScopeIPv4Reservations;
        private ParseTreeProperty<IPv4Lease[]> ScopeIPv4Leases;
        private ParseTreeProperty<IPAddress> Ipv4LeaseAddresses;
        private ParseTreeProperty<IPAddress> Ipv4LeaseScopeIds;
        private ParseTreeProperty<string> Ipv4LeaseClientIds;
        private ParseTreeProperty<string> Ipv4LeaseAddressStates;
        private ParseTreeProperty<bool> Ipv4LeaseNapCapables;
        private ParseTreeProperty<string> Ipv4LeaseDnsRRs;
        private ParseTreeProperty<string> Ipv4LeaseDnsRegistrations;
        private ParseTreeProperty<DateTime> Ipv4LeaseExpiryTimes;
        private ParseTreeProperty<DateTime> Ipv4LeaseProbationEnds;
        private ParseTreeProperty<DateTime> Ipv4LeaseNapStatuses;
        private ParseTreeProperty<string> Ipv4LeaseHostnames;
        private ParseTreeProperty<string> Ipv4LeasePolicyNames;
        private ParseTreeProperty<string> Ipv4LeaseDescriptions;
        private ParseTreeProperty<string> Ipv4FilterLists;
        private ParseTreeProperty<string> Ipv4FilterMacAddresses;
        private ParseTreeProperty<string> Ipv4FilterDescriptions;
        public Logger logger;
        private XmlSerializer xmlSerializer;

        public DhcpdToMicrosoftCompiler() :base()
        {
            List<IPv4Lease> Leases;
            List<IPv4Filter> Filters;
            List<IPv4Reservation> Reservations;
            Scopesv4 = new List<ScopeIPv4>();
            List<IPRange> IpRanges;
            logger = LogManager.GetCurrentClassLogger(); 
            IPAddresses = new ParseTreeProperty<IPAddress>();    
            BindingStates = new ParseTreeProperty<string>();       
            ClassNames = new ParseTreeProperty<string>();       
            ClassTypes = new ParseTreeProperty<string>();       
            ClassData = new ParseTreeProperty<string>();       
            ClassDescriptions = new ParseTreeProperty<string>();       
            ClassVendorID = new ParseTreeProperty<int>();          
            OptionDefinitionNames = new ParseTreeProperty<string>();       
            OptionIDs = new ParseTreeProperty<int>();          
            OptionTypes = new ParseTreeProperty<string>();       
            OptionMultivalued = new ParseTreeProperty<bool>();         
            OptionDescriptions = new ParseTreeProperty<string>();       
            OptionVendorClasses = new ParseTreeProperty<string>();       
            OptionDefaultValues = new ParseTreeProperty<string>();      
            PolicyNames = new ParseTreeProperty<string>();       
            PolicyProcessingOrders = new ParseTreeProperty<int>();          
            PolicyEnabled = new ParseTreeProperty<bool>();         
            PolicyConditions = new ParseTreeProperty<string>();       
            PolicyDescriptions = new ParseTreeProperty<string>();       
            PolicyVendorClasses = new ParseTreeProperty<string>();       
            PolicyVendorClassColumns = new ParseTreeProperty<string>();       
            PolicyUserClassColumns = new ParseTreeProperty<string>();       
            PolicyMacAddressColumns = new ParseTreeProperty<string>();       
            PolicyClientIdColumns = new ParseTreeProperty<string>();       
            PolicyRelayAgentColumns = new ParseTreeProperty<string>();       
            PolicyCircuitIdColumns = new ParseTreeProperty<string>();       
            PolicyRemoteIds = new ParseTreeProperty<string>();       
            PolicySubscriberIds = new ParseTreeProperty<string>();       
            IpRangeStartRanges = new ParseTreeProperty<IPAddress>();    
            IpRangeEndRanges = new ParseTreeProperty<IPAddress>();    
            OptValueOptionIds = new ParseTreeProperty<int>();          
            OptValueOptionVendorClassStrings = new ParseTreeProperty<string>();       
            OptValueOptionUserClassStrings = new ParseTreeProperty<string>();       
            OptValueOptionValues = new ParseTreeProperty<string>();      
            OptValueOptionValueColumns = new ParseTreeProperty<string>();       
            ScopeIPv4ScopeIDs = new ParseTreeProperty<IPAddress>();    
            ScopeIPv4Names = new ParseTreeProperty<string>();       
            ScopeIPv4SubnetMasks = new ParseTreeProperty<string>();       
            ScopeIPv4StartRanges = new ParseTreeProperty<string>();       
            ScopeIPv4EndRanges = new ParseTreeProperty<string>();       
            ScopeIPv4LeaseDurations = new ParseTreeProperty<string>();       
            ScopeIPv4States = new ParseTreeProperty<string>();       
            ScopeIPv4Types = new ParseTreeProperty<string>();       
            ScopeIPv4MaxBootpClients = new ParseTreeProperty<int>();          
            ScopeIPv4NapEnables = new ParseTreeProperty<bool>();         
            ScopeIPv4Descriptions = new ParseTreeProperty<string>();       
            ScopeIPv4ActivatePolicies = new ParseTreeProperty<bool>();         
            ScopeIPv4SuperscopeNames = new ParseTreeProperty<string>();       
            ScopeIPv4ExclusionRanges = new ParseTreeProperty<IPRange[]>();    
            ScopeIPv4Policies = new ParseTreeProperty<Policies>();     
            ScopeIPv4OptionValues = new ParseTreeProperty<OptionValues>();
            ScopeIPv4Reservations = new ParseTreeProperty<IPv4Reservation[]>();
            ScopeIPv4Leases = new ParseTreeProperty<IPv4Lease[]>();  
            Ipv4LeaseAddresses = new ParseTreeProperty<IPAddress>();    
            Ipv4LeaseScopeIds = new ParseTreeProperty<IPAddress>();    
            Ipv4LeaseAddressStates = new ParseTreeProperty<string>();       
            Ipv4LeaseClientIds = new ParseTreeProperty<string>();       
            Ipv4LeaseNapCapables = new ParseTreeProperty<bool>();         
            Ipv4LeaseDnsRRs = new ParseTreeProperty<string>();       
            Ipv4LeaseDnsRegistrations = new ParseTreeProperty<string>();       
            Ipv4LeaseExpiryTimes = new ParseTreeProperty<DateTime>();     
            Ipv4LeaseProbationEnds = new ParseTreeProperty<DateTime>();     
            Ipv4LeaseNapStatuses = new ParseTreeProperty<DateTime>();     
            Ipv4LeaseHostnames = new ParseTreeProperty<string>();       
            Ipv4LeasePolicyNames = new ParseTreeProperty<string>();       
            Ipv4LeaseDescriptions = new ParseTreeProperty<string>();       
            Ipv4FilterLists = new ParseTreeProperty<string>();       
            Ipv4FilterMacAddresses = new ParseTreeProperty<string>();       
            Ipv4FilterDescriptions = new ParseTreeProperty<string>(); 
        }

        private void CompileMsDhcp()
        {
            foreach(IPv4Lease lease in Leases)
            {

            }
        }

        private void ResolveScope()
        {

        }

        public Object VisitRangeLow6([NotNull] DHCPDConfigParser.RangeLow6Context context) { return VisitChildren(context); }
        public Object VisitNetmask([NotNull] DHCPDConfigParser.NetmaskContext context) {
            return context.ip4Address().Ip4Address().GetText();
        }
        public Object VisitSubnet6Declaration([NotNull] DHCPDConfigParser.Subnet6DeclarationContext context) { return VisitChildren(context); }
        public Object VisitSharedNetwork([NotNull] DHCPDConfigParser.SharedNetworkContext context)
        {

            return VisitChildren(context); 
        }
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
        public Object VisitIp4Address([NotNull] DHCPDConfigParser.Ip4AddressContext context)
        {
            
            return context.Ip4Address().GetText();
        }
        public Object VisitStatement([NotNull] DHCPDConfigParser.StatementContext context) { return VisitChildren(context); }
        public Object VisitLease([NotNull] DHCPDConfigParser.LeaseContext context)
        { 
            IPv4Lease lease = new IPv4Lease();
        //    lease.IPAddress = IPAddresses.Get(context).ToString();
          //  lease.AddressState = BindingStates.Get(context);
//            lease.LeaseExpiryTime = DateTime.Parse(context.ExpiryDate);
            //lease.ClientId = NormaliseMAC(context.ClientID);
            //lease.NapCapable = false;
            //lease.ClientType = "Dhcp";
            //lease.NapStatus = "FullAccess";
            //leases.Add(lease);
            return lease.ToString();
        }
            
            
            
        public Object VisitOptionOptionStatement([NotNull] DHCPDConfigParser.OptionOptionStatementContext context) { return VisitChildren(context); }
        public Object VisitOptionParam([NotNull] DHCPDConfigParser.OptionParamContext context) { return VisitChildren(context); }
        public Object VisitIpAddressWithSubnet([NotNull] DHCPDConfigParser.IpAddressWithSubnetContext context) { return VisitChildren(context); }
        public Object VisitFixedAddress([NotNull] DHCPDConfigParser.FixedAddressContext context) { return VisitChildren(context); }
        public Object VisitLeaseParameters([NotNull] DHCPDConfigParser.LeaseParametersContext context) { return VisitChildren(context); }
        public Object VisitSharedNetworkDeclaration([NotNull] DHCPDConfigParser.SharedNetworkDeclarationContext context) {
            logger.Debug("Checking shit");

            List<ParserRuleContext> subnets = GetNestedChildren(context, new string[4]{"StatementsContext","StatementContext","DeclarationContext","SubnetDeclarationContext"});
            AddToNestedChildrenProperty(subnets, context.sharedNetwork().STRING().GetText().ToString(), ScopeIPv4SuperscopeNames);
            return VisitChildren(context);
        }


        private void AddToNestedChildrenProperty(List<ParserRuleContext> Key, string Value, ParseTreeProperty<string> Bag)
        {
            foreach (ParserRuleContext c in Key)
            {
                if(!String.IsNullOrEmpty(Value))
                { 
                    Bag.Put(c, Value);
                    logger.Debug("Adding " + Value + " to " + c.GetType().Name);
                }
            }
        }

        private List<ParserRuleContext> GetNestedChildren(ParserRuleContext context, string[] levels)
        {
            List<ParserRuleContext> list = new List<ParserRuleContext>();
            GetNestedChildrenIterator(context, levels, list);
            return list;
        }

        private void GetNestedChildrenIterator(ParserRuleContext context, string[] levels, IList<ParserRuleContext> list)
        {
            if (levels.Length != 1)
            {
                string[] next = levels.Skip(1).ToArray();
                foreach (ParserRuleContext c in context.children.Where(x => x.GetType().Name.Equals(levels[0])))
                {
                    logger.Debug("Iterating " + c.GetType().Name);
                    GetNestedChildrenIterator(c, next, list);
                }
            }
            else
            {
                foreach (ParserRuleContext c in context.children.Where(x => x.GetType().Name.Equals(levels[0])))
                {
                    logger.Debug("Adding " + c.GetType().Name);
                    list.Add(c);
                }
            }
        }
        public Object VisitOptionStatement([NotNull] DHCPDConfigParser.OptionStatementContext context) { return VisitChildren(context); }
        public Object VisitSubnetDeclaration([NotNull] DHCPDConfigParser.SubnetDeclarationContext context)
        {
            ScopeIPv4 scope = new ScopeIPv4();
            scope.ScopeId = context.subnet().GetText();
            logger.Debug(scope.ScopeId);
            scope.SubnetMask = context.netmask().GetText();
            logger.Debug(scope.SubnetMask);
            try
            {
                string superscope = scope.SuperScopeName = ScopeIPv4SuperscopeNames.Get(context);
                if (!string.IsNullOrEmpty(superscope))
                {
                    logger.Debug("Superscope " + scope.SuperScopeName);
                    scope.SuperScopeName = superscope;
                }
                
            }
            catch
            {
            }
            scope.State = "Active";
            scope.MaxBootpClients = "4294967295";
            Scopesv4.Add(scope);
            return VisitChildren(context);
        }
        public Object VisitLeaseTime([NotNull] DHCPDConfigParser.LeaseTimeContext context){
            return VisitChildren(context);
        }
        public Object VisitFailoverDeclaration([NotNull] DHCPDConfigParser.FailoverDeclarationContext context) { return VisitChildren(context); }
        public Object VisitKlass([NotNull] DHCPDConfigParser.KlassContext context) { return VisitChildren(context); }
        public Object VisitState([NotNull] DHCPDConfigParser.StateContext context) { return VisitChildren(context); }
        public Object VisitGroupDeclaration([NotNull] DHCPDConfigParser.GroupDeclarationContext context) { return VisitChildren(context); }
        public Object VisitDate([NotNull] DHCPDConfigParser.DateContext context) { return VisitChildren(context); }
        public Object VisitTimestamp([NotNull] DHCPDConfigParser.TimestampContext context)
        {

            return VisitChildren(context);
        }
        public Object VisitHostDeclaration([NotNull] DHCPDConfigParser.HostDeclarationContext context)
        {
            IPv4Filter filter = new IPv4Filter();
            filter.Description = context.hostname().STRING().GetText();
            filter.List = "Allow";
            //filter.MacAddress = context.statements().statement().
            return VisitChildren(context); 
        }
        public Object VisitSubnet([NotNull] DHCPDConfigParser.SubnetContext context)
        {
            VisitChildren(context);
   
            return context.ip4Address().GetText();
        }
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
