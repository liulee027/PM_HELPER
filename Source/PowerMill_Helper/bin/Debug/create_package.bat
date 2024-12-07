@echo off
setlocal enabledelayedexpansion

set "PACKAGE_DIR=PowerMill_Helper_Package"

:: Create package directory
if exist "%PACKAGE_DIR%" rd /s /q "%PACKAGE_DIR%"
mkdir "%PACKAGE_DIR%"

:: Copy installation files
copy "PowerMill_Helper.dll" "%PACKAGE_DIR%\"
copy "*.dll" "%PACKAGE_DIR%\"
copy "install.bat" "%PACKAGE_DIR%\"
copy "uninstall.bat" "%PACKAGE_DIR%\"
copy "check_dotnet.bat" "%PACKAGE_DIR%\"

:: Create readme file
echo PowerMill Helper Plugin Installation Package > "%PACKAGE_DIR%\README.txt"
echo. >> "%PACKAGE_DIR%\README.txt"
echo Installation Instructions: >> "%PACKAGE_DIR%\README.txt"
echo 1. Right-click on install.bat and select "Run as administrator" >> "%PACKAGE_DIR%\README.txt"
echo 2. Follow the on-screen instructions >> "%PACKAGE_DIR%\README.txt"
echo. >> "%PACKAGE_DIR%\README.txt"
echo Requirements: >> "%PACKAGE_DIR%\README.txt"
echo - Windows 7 or later >> "%PACKAGE_DIR%\README.txt"
echo - .NET Framework 4.8.1 >> "%PACKAGE_DIR%\README.txt"
echo. >> "%PACKAGE_DIR%\README.txt"
echo To uninstall: >> "%PACKAGE_DIR%\README.txt"
echo 1. Right-click on uninstall.bat and select "Run as administrator" >> "%PACKAGE_DIR%\README.txt"

echo Package created successfully in %PACKAGE_DIR% folder!
pause
