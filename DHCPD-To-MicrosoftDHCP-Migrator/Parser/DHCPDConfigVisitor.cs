//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.1-SNAPSHOT
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from I:\Work\DHCPD-To-MicrosoftDHCP-Migrator\DHCPD-To-MicrosoftDHCP-Migrator\ANTLR\DHCPDConfig.g4 by ANTLR 4.1-SNAPSHOT
namespace DhcpdToMicrosoft.Parser
{
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="DHCPDConfigParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.1-SNAPSHOT")]
public interface IDHCPDConfigVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.rangeLow6"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRangeLow6([NotNull] DHCPDConfigParser.RangeLow6Context context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.netmask"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNetmask([NotNull] DHCPDConfigParser.NetmaskContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.subnet6Declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubnet6Declaration([NotNull] DHCPDConfigParser.Subnet6DeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.sharedNetwork"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSharedNetwork([NotNull] DHCPDConfigParser.SharedNetworkContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.config"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitConfig([NotNull] DHCPDConfigParser.ConfigContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.rangeHigh"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRangeHigh([NotNull] DHCPDConfigParser.RangeHighContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDeclaration([NotNull] DHCPDConfigParser.DeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.fixedPrefix6"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFixedPrefix6([NotNull] DHCPDConfigParser.FixedPrefix6Context context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.hostname"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHostname([NotNull] DHCPDConfigParser.HostnameContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.addressRangeDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddressRangeDeclaration([NotNull] DHCPDConfigParser.AddressRangeDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.addressRange6Declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAddressRange6Declaration([NotNull] DHCPDConfigParser.AddressRange6DeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.leaseAddress"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLeaseAddress([NotNull] DHCPDConfigParser.LeaseAddressContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.rangeLow"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRangeLow([NotNull] DHCPDConfigParser.RangeLowContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.ip4Address"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIp4Address([NotNull] DHCPDConfigParser.Ip4AddressContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatement([NotNull] DHCPDConfigParser.StatementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.lease"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLease([NotNull] DHCPDConfigParser.LeaseContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.optionOptionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionOptionStatement([NotNull] DHCPDConfigParser.OptionOptionStatementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.optionParam"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionParam([NotNull] DHCPDConfigParser.OptionParamContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.ipAddressWithSubnet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIpAddressWithSubnet([NotNull] DHCPDConfigParser.IpAddressWithSubnetContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.fixedAddress"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFixedAddress([NotNull] DHCPDConfigParser.FixedAddressContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.leaseParameters"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLeaseParameters([NotNull] DHCPDConfigParser.LeaseParametersContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.sharedNetworkDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSharedNetworkDeclaration([NotNull] DHCPDConfigParser.SharedNetworkDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.optionStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOptionStatement([NotNull] DHCPDConfigParser.OptionStatementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.subnetDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubnetDeclaration([NotNull] DHCPDConfigParser.SubnetDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.leaseTime"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLeaseTime([NotNull] DHCPDConfigParser.LeaseTimeContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.failoverDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFailoverDeclaration([NotNull] DHCPDConfigParser.FailoverDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.klass"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitKlass([NotNull] DHCPDConfigParser.KlassContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.state"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitState([NotNull] DHCPDConfigParser.StateContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.groupDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGroupDeclaration([NotNull] DHCPDConfigParser.GroupDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.date"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitDate([NotNull] DHCPDConfigParser.DateContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.timestamp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTimestamp([NotNull] DHCPDConfigParser.TimestampContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.hostDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHostDeclaration([NotNull] DHCPDConfigParser.HostDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.subnet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubnet([NotNull] DHCPDConfigParser.SubnetContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.parameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParameter([NotNull] DHCPDConfigParser.ParameterContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.leaseDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLeaseDeclaration([NotNull] DHCPDConfigParser.LeaseDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.failoverStateStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFailoverStateStatement([NotNull] DHCPDConfigParser.FailoverStateStatementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.statements"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStatements([NotNull] DHCPDConfigParser.StatementsContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.fixedAddressParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFixedAddressParameter([NotNull] DHCPDConfigParser.FixedAddressParameterContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.subnet6"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitSubnet6([NotNull] DHCPDConfigParser.Subnet6Context context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.hostnameOrIpAddress"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHostnameOrIpAddress([NotNull] DHCPDConfigParser.HostnameOrIpAddressContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.ip6net"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIp6net([NotNull] DHCPDConfigParser.Ip6netContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.ip6Address"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIp6Address([NotNull] DHCPDConfigParser.Ip6AddressContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.hardwareParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitHardwareParameter([NotNull] DHCPDConfigParser.HardwareParameterContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.stringParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStringParameter([NotNull] DHCPDConfigParser.StringParameterContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.classDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassDeclaration([NotNull] DHCPDConfigParser.ClassDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.rangeHigh6"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRangeHigh6([NotNull] DHCPDConfigParser.RangeHigh6Context context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.ipAddrOrHostnames"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIpAddrOrHostnames([NotNull] DHCPDConfigParser.IpAddrOrHostnamesContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.startEnd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStartEnd([NotNull] DHCPDConfigParser.StartEndContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.leaseParameter"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitLeaseParameter([NotNull] DHCPDConfigParser.LeaseParameterContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.poolDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPoolDeclaration([NotNull] DHCPDConfigParser.PoolDeclarationContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.ip6Prefix"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIp6Prefix([NotNull] DHCPDConfigParser.Ip6PrefixContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.peerStatement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPeerStatement([NotNull] DHCPDConfigParser.PeerStatementContext context);

	/// <summary>
	/// Visit a parse tree produced by <see cref="DHCPDConfigParser.failoverStateDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFailoverStateDeclaration([NotNull] DHCPDConfigParser.FailoverStateDeclarationContext context);
}
} // namespace DHCPD_To_MicrosoftDHCP_Migrator.ANTLR
