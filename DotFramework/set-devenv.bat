ECHO OFF

if "%IsDevenvInPath%"=="" (
    GOTO SETPATH
) else (
    GOTO EXIT
)

:SETPATH

ECHO set env for devenv

SET Path=%Path%;C:\"Program Files (x86)"\"Microsoft Visual Studio"\2019\Enterprise\Common7\IDE
SET IsDevenvInPath=true

:EXIT