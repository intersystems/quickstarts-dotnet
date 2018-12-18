set WORKINGDIR=%CD%
echo set WORKINGDIR=%CD%
echo workdir = "%WORKINGDIR%" > %temp%\sudo.tmp.vbs
echo Set objShell = CreateObject("Shell.Application") >> %temp%\sudo.tmp.vbs

rem Run the following the first time you setup or run devenv /setup from an administrator prompt to setup VS registry
rem echo args = "/X /K cd "+workdir+"&&Installer.exe -confirm true -whatIf false -verbose true -install true -installflags SetupVS" >> %temp%\sudo.tmp.vbs
echo args = "/X /K cd "+workdir+"&&Installer.exe -confirm true -whatIf false -verbose true -install true" >> %temp%\sudo.tmp.vbs
echo objShell.NameSpace(workdir)  >> %temp%\sudo.tmp.vbs
ver | findstr /C:"Microsoft Windows XP" > nul
IF %ERRORLEVEL% EQU 0 goto XP
ver | findstr /c:"Version 5" > nul
IF %ERRORLEVEL% EQU 0 goto noadmin
echo objShell.ShellExecute "CMD", args, "", "runas" >> %temp%\sudo.tmp.vbs
cscript %temp%\sudo.tmp.vbs
goto End

:XP
echo objShell.ShellExecute "CMD", args, "", "" >> %temp%\sudo.tmp.vbs
cscript %temp%\sudo.tmp.vbs
goto End

:noadmin
echo "Must be able to run with administrative privileges to install!"

:End