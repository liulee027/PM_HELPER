@echo off
setlocal enabledelayedexpansion

echo Checking administrator privileges...
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo Please run this uninstaller as administrator!
    pause
    exit /b 1
)

set "INSTALL_DIR=%ProgramFiles%\PowerMill_Helper"

:: Unregister DLL
echo Unregistering DLL...
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe" (
    "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe" "%INSTALL_DIR%\PowerMill_Helper.dll" /unregister
)

:: Remove registry entry
echo Removing registry entries...
reg delete "HKCR\CLSID\{BC3610A0-A0F6-4244-8053-A99AADE569F5}\Implemented Categories\{311b0135-1826-4a8c-98de-f313289f815e}" /reg:64 /f

:: Remove installation directory
echo Removing installed files...
if exist "%INSTALL_DIR%" rd /s /q "%INSTALL_DIR%"

echo Uninstallation completed successfully!
pause
