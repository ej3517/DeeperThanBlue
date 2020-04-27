@echo OFF
SETLOCAL

set warning1=0
python --version >NUL
IF %ERRORLEVEL% NEQ 0 goto TRYPYTHON3
for /f "tokens=*" %%a in ('python --version') do set pyResult=%%a
set pyVersion=%pyResult:~7,1%
set /a pyVersion=%pyVersion%
echo %pyVersion%
IF %pyVersion% LEQ 2 goto TRYPYTHON3
echo Python installed
goto TRYPIP3

:TRYPYTHON3
python3 --version >NUL
IF %ERRORLEVEL% NEQ 0 goto MISSINGPYTHON
set warning1=1
echo Python3 installed

::CHECK VERSION OF PIP
:TRYPIP3
pip3 --version >NUL
IF %ERRORLEVEL% NEQ 0 goto TRYPIP
SET PIP_type=pip3
goto update

:TRYPIP
pip --version >NUL
IF %ERRORLEVEL% NEQ 0 goto MISSINGPIP
SET PIP_type=pip
goto update

:UPDATE
set error=0
echo Update using: %PIP_type%
%PIP_type% install --upgrade "ibm-watson>=4.4.0"
IF %ERRORLEVEL% NEQ 0 set error=error+1
%PIP_type% install -U python-dotenv
IF %ERRORLEVEL% NEQ 0 set error2=error+1

echo;
if %error% EQU 0 (echo Successfully installed all packages) else (echo %error% packages were not installed successfully)
if %warning1% EQU 1 (echo Make sure to use python3)
goto end

:MISSINGPIP
echo Pip was missing
goto end

:MISSINGPYTHON
echo Python 3 is not installed. Would you still like to install the packages?
CHOICE /c YN /t 30 /D Y /m "YES or NO" 
echo %ERRORLEVEL%
IF %ERRORLEVEL% EQU 1 goto TRYPIP3
IF %ERRORLEVEL% EQU 2 echo Stopping installation...
ENDLOCAL

:END