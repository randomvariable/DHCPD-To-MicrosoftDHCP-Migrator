
namespace DhcpdToMicrosoft.Compiler
{
    using Antlr4.Runtime;
    using Antlr4.Runtime.Misc;
    using Antlr4.Runtime.Tree;
    using DhcpdToMicrosoft.Model;
    using DhcpdToMicrosoft.Parser;
    using DhcpdToMicrosoft.Utility;
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Xml.Serialization;
    using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

    public class DhcpdToMicrosoftCompiler : AbstractParseTreeVisitor<Object>, IDHCPDConfigVisitor<Object>
    {
        private HashSet<IPv4Lease> Leases;
        private HashSet<IPv4Filter> Filters;
        private HashSet<IPv4Reservation> Reservations;
        private List<ScopeIPv4> Scopesv4;
        private ParseTreeProperty<string> Ipv4LeaseClientTypes;
        private IEnumerable<IPAddress> ExcludedIPs;
        private IEnumerable<IPAddress> IncludedIPs;
        private ParseTreeProperty<string> ScopeIPv4SuperscopeNames;
        private ParseTreeProperty<string> Ipv4LeaseClientIds;
        private ParseTreeProperty<string> Ipv4LeaseAddressStates;
        private ParseTreeProperty<DateTime> Ipv4LeaseExpiryTimes;
        private ParseTreeProperty<string> Ipv4LeaseHostnames;
        private ParseTreeProperty<string> RouterProperties;
        private ParseTreeProperty<Dictionary<string, IEnumerable<string>>> ContextOptions;
        private Logger logger;

        public DhcpdToMicrosoftCompiler()
            : base()
        {
            Leases = new HashSet<IPv4Lease>();
            Filters = new HashSet<IPv4Filter>();
            Reservations = new HashSet<IPv4Reservation>();
            Scopesv4 = new List<ScopeIPv4>();
            logger = LogManager.GetCurrentClassLogger();
            Ipv4LeaseClientTypes = new ParseTreeProperty<string>();
            ExcludedIPs = new HashSet<IPAddress>();
            IncludedIPs = new HashSet<IPAddress>();
            ScopeIPv4SuperscopeNames = new ParseTreeProperty<string>();
            Ipv4LeaseAddressStates = new ParseTreeProperty<string>();
            Ipv4LeaseClientIds = new ParseTreeProperty<string>();
            Ipv4LeaseExpiryTimes = new ParseTreeProperty<DateTime>();
            Ipv4LeaseHostnames = new ParseTreeProperty<string>();
            RouterProperties = new ParseTreeProperty<string>();
            ContextOptions = new ParseTreeProperty<Dictionary<string, IEnumerable<string>>>();
        }



        public Object VisitNetmask([NotNull] DHCPDConfigParser.NetmaskContext context)
        {
            return context.ip4Address().Ip4Address().GetText();
        }
        public Object VisitAddressRangeDeclaration([NotNull] DHCPDConfigParser.AddressRangeDeclarationContext context)
        {
            IPAddress low = IPAddress.Parse((context.rangeLow()
                .ip4Address().Ip4Address().GetText()));
            IPAddress high = IPAddress.Parse((context.rangeHigh()
                .ip4Address().Ip4Address().GetText()));
            IEnumerable<IPAddress> rangeIncludedIPs = Utility.IPAddressExtensions.GetIPRange(low, high);
            IncludedIPs = IncludedIPs.Concat(rangeIncludedIPs);
            return VisitChildren(context);
        }
        public Object VisitIp4Address([NotNull] DHCPDConfigParser.Ip4AddressContext context)
        {
            return context.Ip4Address().GetText();
        }

        public Object VisitLease([NotNull] DHCPDConfigParser.LeaseContext context)
        {
            return VisitChildren(context);
        }


        public Object VisitSharedNetworkDeclaration([NotNull] DHCPDConfigParser.SharedNetworkDeclarationContext context)
        {
            List<ParserRuleContext> subnets = GetNestedChildren(context
                , new string[4]{"StatementsContext","StatementContext"
                    ,"DeclarationContext","SubnetDeclarationContext"});
            AddToNestedChildrenProperty(subnets, context.sharedNetwork()
                .STRING().GetText().ToString(), ScopeIPv4SuperscopeNames);
            return VisitChildren(context);
        }


        private void AddToNestedChildrenProperty(List<ParserRuleContext> Key, string Value, ParseTreeProperty<string> Bag)
        {
            foreach (ParserRuleContext c in Key)
            {
                if (!String.IsNullOrEmpty(Value))
                {
                    Bag.Put(c, Value);
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
                foreach (ParserRuleContext c in context.children.Where(x =>
                    x.GetType().Name.Equals(levels[0])))
                {
                    GetNestedChildrenIterator(c, next, list);
                }
            }
            else
            {
                foreach (ParserRuleContext c in context.children.Where(x =>
                    x.GetType().Name.Equals(levels[0])))
                {
                    list.Add(c);
                }
            }
        }
        public Object VisitSubnetDeclaration([NotNull] DHCPDConfigParser.SubnetDeclarationContext context)
        {
            VisitChildren(context);
            ScopeIPv4 scope = new ScopeIPv4();
            scope.ScopeId = context.subnet().GetText();
            scope.Name = scope.ScopeId;
            scope.SubnetMask = context.netmask().GetText();
            try
            {
                string superscope = scope.SuperScopeName = ScopeIPv4SuperscopeNames.Get(context);
                if (!string.IsNullOrEmpty(superscope))
                {
                    scope.SuperScopeName = superscope;
                }

            }
            catch
            {
            }
            IPAddress startRange = IPAddress.Parse(scope.ScopeId).GetFirstUsuableAddress();
            IPAddress endRange = (IPAddress.Parse(scope.ScopeId)
                .GetLastAddress(IPAddress.Parse(scope.SubnetMask)));
            scope.StartRange = startRange.ToString();
            scope.EndRange = endRange.ToString();
            IEnumerable<IPAddress> subnetExcludedIPs = IPAddressExtensions.GetIPRange(startRange, endRange);
            ExcludedIPs = ExcludedIPs.Concat(subnetExcludedIPs);
            scope.State = "Active";
            scope.MaxBootpClients = "4294967295";
            scope.LeaseDuration = "3";
            scope.Type = "Both";
            Scopesv4.Add(scope);


            Dictionary<string, IEnumerable<string>> dict = ContextOptions.Get(context);
            if (dict != null)
            {
                HashSet<OptValue> options = new HashSet<OptValue>();
                foreach (KeyValuePair<string, IEnumerable<string>> keyPair in dict)
                {
                    OptValue opt = new OptValue();
                    opt.OptionId = keyPair.Key;
                    HashSet<OptValueValue> valueValues = new HashSet<OptValueValue>();
                    foreach(string s in keyPair.Value)
                    {
                        OptValueValue value = new OptValueValue();
                        value.Value = s;
                        valueValues.Add(value);
                    }
                    opt.Value = valueValues.ToArray();
                    options.Add(opt);
                }
                scope.OptionValues = options.ToArray();
            }
            
               
            return VisitChildren(context);
        }
        

        public Object VisitLeaseTime([NotNull] DHCPDConfigParser.LeaseTimeContext context)
        {
            return VisitChildren(context);
        }
        public Object VisitFailoverDeclaration([NotNull] DHCPDConfigParser.FailoverDeclarationContext context)
        {
            // Not transferable to MS.
            return VisitChildren(context);
        }

        public Object VisitGroupDeclaration([NotNull] DHCPDConfigParser.GroupDeclarationContext context)
        {
            // Do nothing. Not relevant to Microsoft.
            return VisitChildren(context);

        }


        public Object VisitTimestamp([NotNull] DHCPDConfigParser.TimestampContext context)
        {
            RuleContext declaration = context.parent.parent.parent;
            string endDateString = context.Date().GetText();
            DateTime normalisedTime = DateTime.Parse(endDateString);
            if (Ipv4LeaseExpiryTimes.Get(declaration) != null)
            {
                if ((Ipv4LeaseExpiryTimes.Get(declaration) < normalisedTime)
                    & (DateTime.Now < normalisedTime))
                {
                    Ipv4LeaseExpiryTimes.RemoveFrom(declaration);
                    Ipv4LeaseExpiryTimes.Put(declaration, normalisedTime);
                }
            }


            return VisitChildren(context);
        }

        private string normaliseMac(string mac)
        {
            return mac.Replace(":", "-");
        }

        public Object VisitHostDeclaration([NotNull] DHCPDConfigParser.HostDeclarationContext context)
        {
            IPv4Filter filter = new IPv4Filter();
            filter.List = "Allow";
            List<ParserRuleContext> hardwareParameters = GetNestedChildren(context,
                new String[] { "StatementsContext", "StatementContext", "ParameterContext",
                    "HardwareParameterContext" });
            if (hardwareParameters != null)
            {
                filter.MacAddress = normaliseMac(hardwareParameters.First().GetChild(2).GetText());
            }
            if (context.hostname().STRING() != null)
            {
                filter.Description = context.hostname().STRING().GetText();
            }
            else
            {
                filter.Description = normaliseMac(filter.MacAddress);
            }
            try
            {
                if ((Filters.First(x => x.MacAddress == filter.MacAddress)) != null)
                {
                    logger.Warn("Duplicate MAC Address in filter: " + normaliseMac(filter.MacAddress));
                }
                else
                {
                    Filters.Add(filter);
                }
            }
            catch
            {
                Filters.Add(filter);
            }
            List<ParserRuleContext> fixedAddressParameters = GetNestedChildren(context,
                new String[] { "StatementsContext", "StatementContext",
                    "ParameterContext", "FixedAddressParameterContext" });
            if (fixedAddressParameters != null & fixedAddressParameters.Count > 0)
            {
                IPv4Reservation reservation = new IPv4Reservation();
                reservation.ClientId = filter.MacAddress;
                reservation.Name = filter.Description;
                reservation.IPAddress = (Visit(fixedAddressParameters.First())).ToString();
                try
                {
                    if ((Reservations.First(x => x.IPAddress == reservation.IPAddress)) != null)
                    {
                        logger.Warn("Duplicate IP Address in reservations: " + reservation.IPAddress);
                    }
                    else
                    {
                        Reservations.Add(reservation);
                    }
                }
                catch
                {
                    Reservations.Add(reservation);
                }
            }
            return VisitChildren(context);
        }
        public Object VisitOptionStatement([NotNull] DHCPDConfigParser.OptionStatementContext context) {
            if (context.optionParam() != null)
            {
                if (context.optionParam().stringParameter() != null)
                {
                    foreach (DHCPDConfigParser.StringParameterContext sc in context.optionParam().stringParameter())
                    {
                        DHCPDConfigParser.OptionStatementContext opt = (DHCPDConfigParser.OptionStatementContext)sc.parent.parent;
                        RuleContext topLevelParent = context.parent.parent.parent.parent;
                        if (topLevelParent != null)
                        {
                            if (ContextOptions.Get(topLevelParent) == null)
                            {
                                ContextOptions.Put(topLevelParent, new Dictionary<string, IEnumerable<string>>());
                            }
                            switch (sc.STRING().GetText())
                            {
                                case "routers":
                                    string router = opt.hostnameOrIpAddress().ip4Address().Ip4Address().GetText();
                                    HashSet<string> values = new HashSet<string>();
                                    values.Add(router);
                                    if (!ContextOptions.Get(topLevelParent).ContainsKey("3"))
                                    {
                                        ContextOptions.Get(topLevelParent).Add("3", values);
                                    }
                                    break;
                                case "domain-name":
                                    IEnumerable<string> stringsA = opt.optionParam().stringParameter().Select(x => x.STRING().GetText());
                                    HashSet<string> exclusionStrings = new HashSet<string>();
                                    exclusionStrings.Add(",");
                                    exclusionStrings.Add("domain-name");
                                    IEnumerable<string> selectedStrings = stringsA.Except(exclusionStrings);
                                    if (!ContextOptions.Get(topLevelParent).ContainsKey("15"))
                                    {
                                        ContextOptions.Get(topLevelParent).Add("15", selectedStrings);
                                    }
                                    break;
                            }
                        }
                    }
                }
            }



            return VisitChildren(context); }



        public Object VisitSubnet([NotNull] DHCPDConfigParser.SubnetContext context)
        {
            VisitChildren(context);

            return context.ip4Address().GetText();
        }

        public Object VisitLeaseDeclaration([NotNull] DHCPDConfigParser.LeaseDeclarationContext context)
        {
            VisitChildren(context);
            if (Ipv4LeaseExpiryTimes.Get(context) > DateTime.Now)
            {
                IPv4Lease lease = new IPv4Lease();
                lease.IPAddress = context.leaseAddress().ip4Address().Ip4Address().GetText();
                lease.LeaseExpiryTime = Ipv4LeaseExpiryTimes.Get(context).ToString("o");
                lease.ClientId = Ipv4LeaseClientIds.Get(context);
                lease.HostName = Ipv4LeaseHostnames.Get(context);
                lease.NapStatus = "FullAccess";
                lease.NapCapable = false;
                lease.ClientType = Ipv4LeaseClientTypes.Get(context);
                lease.AddressState = Ipv4LeaseAddressStates.Get(context);
                if (String.IsNullOrEmpty(lease.ClientType))
                {
                    lease.ClientType = "Unspecified";
                }
                if (String.IsNullOrEmpty(lease.DnsRegistration))
                {
                    lease.DnsRegistration = "NotApplicable";
                }
                if (String.IsNullOrEmpty(lease.DnsRR))
                {
                    lease.DnsRR = "NoRegistration";
                }
                if (String.IsNullOrEmpty(lease.HostName))
                {
                    lease.HostName = lease.ClientId.Replace("-", "");
                }
                if (String.IsNullOrEmpty(lease.AddressState))
                {
                    lease.AddressState = "Active";
                }
                try
                {

                    if ((Leases.First(x => x.IPAddress == lease.IPAddress)) != null)
                    {
                        logger.Warn("Duplicate IP Address in lease: " + lease.IPAddress +
                            " and MAC " + lease.ClientId + " with expiry time at " +
                            (Leases.First(x => x.IPAddress == lease.IPAddress).LeaseExpiryTime));
                        if (DateTime.Parse(Leases.First(x => x.IPAddress == lease
                            .IPAddress).LeaseExpiryTime) < DateTime.Parse(lease.LeaseExpiryTime))
                        {
                            Leases.Remove(Leases.First(x => x.IPAddress == lease.IPAddress));
                            Leases.Add(lease);
                            logger.Debug("Replacement lease " + lease.IPAddress +
                                " with hostname " + lease.HostName + " and MAC "
                                + lease.ClientId + " expires later on "
                                + lease.LeaseExpiryTime);
                        }

                    }
                    else
                    {
                        Leases.Add(lease);
                        logger.Debug("Lease " + lease.IPAddress + " with hostname " +
                            lease.HostName + " and MAC " + lease.ClientId +
                            " expires on " + lease.LeaseExpiryTime);
                    }
                }
                catch
                {
                    Leases.Add(lease);
                    logger.Debug("Lease " + lease.IPAddress + " with hostname " +
                        lease.HostName + " and MAC " + lease.ClientId +
                        " expires on " + lease.LeaseExpiryTime);
                }

            }
            return null;
        }

        public Object VisitFixedAddressParameter([NotNull] DHCPDConfigParser.FixedAddressParameterContext context)
        {

            return context.fixedAddress().ip4Address().Ip4Address().GetText();

        }

        public Object VisitStartEnd([NotNull] DHCPDConfigParser.StartEndContext context)
        {


            if (context.ENDS() != null)
            {
                RuleContext declaration = context.parent.parent.parent;
                string endDateString = context.Date().GetText();
                DateTime normalisedTime = DateTime.Parse(endDateString);
                if (Ipv4LeaseExpiryTimes.Get(declaration) != null)
                {
                    if (Ipv4LeaseExpiryTimes.Get(declaration) < normalisedTime & (DateTime.Now < normalisedTime))
                    {
                        Ipv4LeaseExpiryTimes.RemoveFrom(declaration);
                        Ipv4LeaseExpiryTimes.Put(declaration, normalisedTime);
                    }
                }
            }


            return VisitChildren(context);

        }
        public Object VisitLeaseParameter([NotNull] DHCPDConfigParser.LeaseParameterContext context)
        {
            if (context.CLIENT_HOSTNAME() != null)
            {
                string hostName = context.stringParameter().STRING().GetText();
                Ipv4LeaseHostnames.Put(context.parent.parent, hostName);
            }

            if (context.hardwareParameter() != null)
            {
                Ipv4LeaseClientIds.Put(context.parent.parent, normaliseMac(context
                    .hardwareParameter().ColonSeparatedList().GetText()));
            }

            return VisitChildren(context);

        }


        public string CompileXML()
        {
            NewDataSet dataset = new NewDataSet();

            IEnumerable<IPAddress> fullExclusions = ExcludedIPs.Except(IncludedIPs);
            IDictionary<IPAddress, IPAddress> exclusionRanges = IPAddressExtensions
                .ExtractContiguousRanges(fullExclusions);

            foreach (ScopeIPv4 scope in Scopesv4)
            {
                IPAddress scopeID = IPAddress.Parse(scope.ScopeId);
                IPAddress subnetMask = IPAddress.Parse(scope.SubnetMask);
                List<IPv4Reservation> scopeReservations = new List<IPv4Reservation>();
                foreach (IPv4Reservation reservation in Reservations)
                {
                    IPAddress ipAddress = IPAddress.Parse(reservation.IPAddress);
                    if (ipAddress.IsInSameSubnet(scopeID, subnetMask))
                    {
                        scopeReservations.Add(reservation);
                    }
                }
                scope.Reservations = scopeReservations.ToArray();
                List<IPv4Lease> scopeLeases = new List<IPv4Lease>();
                foreach (IPv4Lease lease in Leases)
                {
                    IPAddress ipAddress = IPAddress.Parse(lease.IPAddress);
                    if (ipAddress.IsInSameSubnet(scopeID, subnetMask))
                    {
                        lease.ScopeId = scope.ScopeId;
                        scopeLeases.Add(lease);
                    }
                }
                scope.Leases = scopeLeases.ToArray();
                HashSet<IPRange> scopeExclusions = new HashSet<IPRange>();
                foreach (KeyValuePair<IPAddress, IPAddress> dict in (exclusionRanges
                    .Where(x => x.Key.IsInSameSubnet(scopeID, subnetMask))))
                {
                    IPRange range = new IPRange();
                    range.StartRange = dict.Key.ToString();
                    range.EndRange = dict.Value.ToString();
                    scopeExclusions.Add(range);
                }
                scope.ExclusionRanges = scopeExclusions.ToArray();
            }
            //dataset

            DHCPServer server = new DHCPServer();
            server.IPv4 = new DHCPv4();
            server.IPv6 = new DHCPv6();
            Filters[] allFilters = new Model.Filters[1];
            allFilters[0] = new Model.Filters();
            allFilters[0].Filter = Filters.ToArray();
            allFilters[0].Allow = false;
            allFilters[0].Deny = false;
            server.IPv4.Filters = allFilters;
            server.IPv4.NapEnabled = false;
            server.MajorVersion = "6";
            server.MinorVersion = "2";
            server.IPv4.ConflictDetectionAttempts = "1";
            server.IPv4.NpsUnreachableAction = "Full";
            server.IPv4.Scopes = Scopesv4.ToArray();
            dataset = new NewDataSet();

            DHCPServer[] serverArray = new DHCPServer[1];
            serverArray[0] = server;
            dataset.Items = serverArray;
            TextWriter writer = new StringWriter();
            XmlSerializer scopeSerializer = new XmlSerializer(typeof(DHCPServer));
            scopeSerializer.Serialize(writer, server);
            return (writer.ToString());
        }
        public Object VisitSubnet6([NotNull] DHCPDConfigParser.Subnet6Context context) { return VisitChildren(context); }
        public Object VisitHostnameOrIpAddress([NotNull] DHCPDConfigParser.HostnameOrIpAddressContext context) { return VisitChildren(context); }
        public Object VisitIp6net([NotNull] DHCPDConfigParser.Ip6netContext context) { return VisitChildren(context); }
        public Object VisitIp6Address([NotNull] DHCPDConfigParser.Ip6AddressContext context) { return VisitChildren(context); }
        public Object VisitHardwareParameter([NotNull] DHCPDConfigParser.HardwareParameterContext context) { return VisitChildren(context); }
        public Object VisitStringParameter([NotNull] DHCPDConfigParser.StringParameterContext context) { return VisitChildren(context); }
        public Object VisitClassDeclaration([NotNull] DHCPDConfigParser.ClassDeclarationContext context) { return VisitChildren(context); }
        public Object VisitRangeHigh6([NotNull] DHCPDConfigParser.RangeHigh6Context context) { return VisitChildren(context); }
        public Object VisitIpAddrOrHostnames([NotNull] DHCPDConfigParser.IpAddrOrHostnamesContext context) { return VisitChildren(context); }
        public Object VisitFailoverStateStatement([NotNull] DHCPDConfigParser.FailoverStateStatementContext context) { return VisitChildren(context); }
        public Object VisitStatements([NotNull] DHCPDConfigParser.StatementsContext context) { return VisitChildren(context); }
        public Object VisitParameter([NotNull] DHCPDConfigParser.ParameterContext context) { return VisitChildren(context); }
        public Object VisitDate([NotNull] DHCPDConfigParser.DateContext context) { return VisitChildren(context); }
        public Object VisitKlass([NotNull] DHCPDConfigParser.KlassContext context) { return VisitChildren(context); }
        public Object VisitState([NotNull] DHCPDConfigParser.StateContext context) { return VisitChildren(context); }
        public Object VisitStatement([NotNull] DHCPDConfigParser.StatementContext context) { return VisitChildren(context); }
        public Object VisitAddressRange6Declaration([NotNull] DHCPDConfigParser.AddressRange6DeclarationContext context) { return VisitChildren(context); }
        public Object VisitLeaseAddress([NotNull] DHCPDConfigParser.LeaseAddressContext context) { return VisitChildren(context); }
        public Object VisitRangeLow([NotNull] DHCPDConfigParser.RangeLowContext context) { return VisitChildren(context); }
        public Object VisitSubnet6Declaration([NotNull] DHCPDConfigParser.Subnet6DeclarationContext context) { return VisitChildren(context); }
        public Object VisitRangeLow6([NotNull] DHCPDConfigParser.RangeLow6Context context) { return VisitChildren(context); }
        public Object VisitSharedNetwork([NotNull] DHCPDConfigParser.SharedNetworkContext context) { return VisitChildren(context); }
        public Object VisitConfig([NotNull] DHCPDConfigParser.ConfigContext context) { return VisitChildren(context); }
        public Object VisitRangeHigh([NotNull] DHCPDConfigParser.RangeHighContext context) { return VisitChildren(context); }
        public Object VisitDeclaration([NotNull] DHCPDConfigParser.DeclarationContext context) { return VisitChildren(context); }
        public Object VisitFixedPrefix6([NotNull] DHCPDConfigParser.FixedPrefix6Context context) { return VisitChildren(context); }
        public Object VisitHostname([NotNull] DHCPDConfigParser.HostnameContext context) { return VisitChildren(context); }
        public Object VisitOptionParam([NotNull] DHCPDConfigParser.OptionParamContext context) { return VisitChildren(context); }
        public Object VisitIpAddressWithSubnet([NotNull] DHCPDConfigParser.IpAddressWithSubnetContext context) { return VisitChildren(context); }
        public Object VisitFixedAddress([NotNull] DHCPDConfigParser.FixedAddressContext context) { return VisitChildren(context); }
        public Object VisitLeaseParameters([NotNull] DHCPDConfigParser.LeaseParametersContext context) { return VisitChildren(context); }
        public Object VisitOptionOptionStatement([NotNull] DHCPDConfigParser.OptionOptionStatementContext context) { return VisitChildren(context); }
        public Object VisitPoolDeclaration([NotNull] DHCPDConfigParser.PoolDeclarationContext context) { return VisitChildren(context); }
        public Object VisitIp6Prefix([NotNull] DHCPDConfigParser.Ip6PrefixContext context) { return VisitChildren(context); }
        public Object VisitPeerStatement([NotNull] DHCPDConfigParser.PeerStatementContext context) { return VisitChildren(context); }
        public Object VisitFailoverStateDeclaration([NotNull] DHCPDConfigParser.FailoverStateDeclarationContext context) { return VisitChildren(context); }
    }
}
