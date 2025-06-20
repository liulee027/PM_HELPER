@echo off
setlocal enabledelayedexpansion

echo Checking administrator privileges...
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Please run this installer as administrator!
    pause
    exit /b 1
)

echo Checking .NET Framework...
call "%~dp0check_dotnet.bat"
if %errorLevel% neq 0 (
    echo .NET Framework 4.8.1 is required. Please install it first.
    start https://dotnet.microsoft.com/download/dotnet-framework/net481
    pause
    exit /b 1
)

set SERVICE_NAME=DynamicPluginService
set BIN_PATH="%~dp0PMH_Service.exe"

rem stop And delete old service
sc stop %SERVICE_NAME% >nul 2>nuls
sc delete %SERVICE_NAME% >nul 2>nul

rem create new service
sc create %SERVICE_NAME% binPath= %BIN_PATH% start= auto DisplayName= "Dynamic Plugin Service" start= auto

rem set service description
sc description %SERVICE_NAME% "PMHelper Service for PowerMill"

rem start service
net start %SERVICE_NAME%

echo Installing PowerMill Helper Plugin...


:: Register DLL
echo Registering DLL...
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\RegAsm.exe" (
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe" "%~dp0PowerMill_Helper.dll" /register /codebase
) else (
    echo Error: RegAsm.exe not found!
    pause
    exit /b 1
)

:: Add registry entry
echo Adding registry entries...
reg add "HKCR\CLSID\{BC3610A0-A0F6-4244-8053-A99AADE569F5}\Implemented Categories\{311b0135-1826-4a8c-98de-f313289f815e}" /reg:64 /f



::echo Installation completed successfully!
::pause
