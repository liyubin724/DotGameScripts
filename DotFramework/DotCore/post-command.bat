SET POST_COMMAND_CONFIG_PATH=%~dp0config.bat

if not exist %POST_COMMAND_CONFIG_PATH% (
    GOTO FINISH
)

CALL %POST_COMMAND_CONFIG_PATH%

del /q %POST_COMMAND_CONFIG_PATH%

ECHO %DLL_PLATFORM%

:FINISH
ECHO finished