@ECHO OFF

SETLOCAL ENABLEDELAYEDEXPANSION

SET BuildMode=Debug
SET BuildLogPath=.\build.log
SET ProjectName=DotCore

IF EXIST %BuildLogPath% (
    DEL /Q %BuildLogPath%%
)

CALL ..\set-devenv.bat

ECHO build start

devenv .\%ProjectName%.sln /rebuild "%BuildMode%" /project "%ProjectName%" /out %BuildLogPath%

ECHO build finish