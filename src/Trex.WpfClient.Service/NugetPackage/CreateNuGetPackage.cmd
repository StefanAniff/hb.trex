@echo off
'.\ildasm.exe /all /out=Test.il "..\Trex.Common\bin\Debug\Trex.Common.dll"
'.\sn.exe -Tp "..\Trex.Common\bin\Debug\Trex.Common.dll"
'.\ilasm /dll /key=Foo.snk Bar.il
.\NuGet.exe pack TrexCommon.nuspec
pause