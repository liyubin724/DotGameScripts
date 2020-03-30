@ECHO OFF

SET BuildMode=Debug
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
)

ECHO build start(%ProjectName%Editor)

devenv .\%ProjectName%.sln /rebuild "%BuildMode%" /project "%ProjectName%Editor" /out %BuildLogPath%

IF ERRORLEVEL 1 (
    GOTO BuildError
) ELSE (
    ECHO build success
    GOTO BuildSuccess
)

:BuildError
ECHO build failed & PAUSE > nul

:BuildSuccess