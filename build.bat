CALL "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"

MSBuild .\HashChecker.sln /p:Configuration=Release /p:Platform=x86
MSBuild .\HashChecker.sln /p:Configuration=Release /p:Platform=x64