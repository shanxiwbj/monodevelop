@echo off
setlocal enableextensions enabledelayedexpansion

rem try to find MSBuild in VS2017
rem the "correct" way is to use a COM API. not easy here.

FOR %%E in (Enterprise, Professional, Community) DO (
	set "MSBUILD_EXE=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\%%E\MSBuild\15.0\Bin\MSBuild.exe"
	if exist "!MSBUILD_EXE!" goto :build
)

rem check for MSBuild from VS2015

set "MSBUILD_EXE=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe"
if exist "%MSBUILD_EXE%" goto :build

echo Could not find MSBuild
exit /b 1

:build

git submodule sync || goto :error
git submodule update --init --recursive || goto :error
"external\nuget-binary\NuGet.exe" restore Main.sln || goto :error

set "CONFIG=DebugWin32"
set "PLATFORM=Any CPU"

# only perform integrated restore on RefactoringEssentials, it fails on the whole solution
"%MSBUILD_EXE%" external\RefactoringEssentials\RefactoringEssentials.2017.sln /target:Restore %* || goto :error

"%MSBUILD_EXE%" Main.sln /m "/p:Configuration=%CONFIG%" "/p:Platform=%PLATFORM%" %* || goto :error
goto :eof

:error

for %%x in (%CMDCMDLINE%) do if /i "%%~x" == "/c" pause
exit /b %ERRORLEVEL%
