del *.java
del *.class
del *.tokens
cd ..\DHCPD-To-MicrosoftDHCP-Migrator\Parser
rem java org.antlr.v4.Tool DHCPLexer.g4
java org.antlr.v4.Tool DHCPDConfig.g4

"c:\Program Files\Java\jdk1.7.0_40\bin\javac.exe" *.java
java org.antlr.v4.runtime.misc.TestRig DHCPDConfig config  -trace -tree -tokens ..\..\ucltestdata\combined.conf > ..\..\ucltestdata\results.txt 2> ..\..\ucltestdata\errors.txt
cd ..\..\tests

