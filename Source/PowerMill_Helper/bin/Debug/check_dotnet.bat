@echo off
setlocal enabledelayedexpansion

:: Check for .NET Framework 4.8.1 or later
for /f "tokens=3" %%i in ('reg query "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v Release') do (
    set "release=%%i"
)

if !release! geq 528040 (
    echo .NET Framework 4.8.1 or later is installed.
    exit /b 0
) else (
    echo .NET Framework 4.8.1 or later is not installed.
    exit /b 1
)
