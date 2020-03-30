ECHO OFF

SETLOCAL enabledelayedexpansion

SET BuildMode=Debug
SET BuildLogPath=.\build.log
SET ProjectName=DotAI

IF EXIST %BuildLogPath% (
    DEL /Q %BuildLogPath%%
)

CALL ..\set-devenv.bat

ECHO build start

devenv .\%ProjectName%.sln /rebuild "%BuildMode%" /project "%ProjectName%" /out %BuildLogPath%

ECHO build finish
