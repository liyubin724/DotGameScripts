@ECHO OFF

SET BuildMode=Release
SET BuildLogPath=.\build.log
SET ProjectName=DotAsset

IF EXIST %BuildLogPath% (
    DEL /Q %BuildLogPath%%
)

CALL ..\set-devenv.bat

ECHO build start(%ProjectName%)

devenv .\%ProjectName%.sln /rebuild "%BuildMode%" /project "%ProjectName%" /out %BuildLogPath%

IF ERRORLEVEL 1 (
    GOTO BuildError
) ELSE (
    ECHO build success
    GOTO BuildSuccess
)

:BuildError
ECHO build failed & PAUSE > nul

:BuildSuccess