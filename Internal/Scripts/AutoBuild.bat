set reposPath = %USERPROFILE%\source\repos\InfrastructureDemo
set outputPath = d:\public\tools
call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"
pushd %reposPath%
git checkout -- .
git checkout master
git pull
msbuild  %reposPath%\InfrastructureDemo.sln /t:Clean,Build 
rmdir %outputPath%
mkdir %outputPath%
xcopy %reposPath%\InfrastructureDemo\bin\Debug %outputPath%