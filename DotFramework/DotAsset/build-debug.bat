@ECHO OFF

SET BuildMode=Debug
SET BuildLogPath=.\build.log
SET ProjectName=DotAsset

IF EXIST %BuildLogPath% (
    DEL /Q %BuildLogPath%%
)

CALL ..\set-devenv.bat

ECHO build start

devenv .\%ProjectName%.sln /rebuild "%BuildMode%" /project "%ProjectName%" /out %BuildLogPath%

ECHO %ERRORLEVEL%

IF ERRORLEVEL 1 (
    ECHO build Error
) ELSE (
    ECHO build success
)

PAUSE