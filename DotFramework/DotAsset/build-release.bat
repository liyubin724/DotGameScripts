@ECHO OFF

SET BuildMode=Release
SET BuildLogPath=.\build.log
SET ProjectName=DotAsset

IF EXIST %BuildLogPath% (
    DEL /Q %BuildLogPath%%
)

CALL ..\set-devenv.bat

ECHO build start

devenv .\%ProjectName%.sln /rebuild "%BuildMode%" /project "%ProjectName%" /out %BuildLogPath%

IF ERRORLEVEL 1 (
    ECHO build Error
) ELSE (
    ECHO build success
)

PAUSE