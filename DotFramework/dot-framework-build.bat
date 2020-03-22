ECHO OFF

SET VSDevenvDir=C:\"Program Files (x86)"\"Microsoft Visual Studio"\2019\Enterprise\Common7\IDE
SET Path=%Path%;%VSDevenvDir%

SET BuildMode=Debug
IF "%BuildMode%"=="Debug" (
    SET BuildLogPath=.\build-debug.log
) else (
    SET BuildLogPath=.\build-release.log
)

IF EXIST %BuildLogPath% (
    DEL /Q %BuildLogPath%
)

REM build DotLog
ECHO ��ʼ����DotLog >> %BuildLogPath%
devenv .\DotLog\DotLog.sln /rebuild "%BuildMode%" /project "DotLog" /out %BuildLogPath%

REM build DotCore
ECHO ��ʼ����DotCore >> %BuildLogPath%
devenv .\DotCore\DotCore.sln /rebuild "%BuildMode%" /project "DotCore" /out %BuildLogPath%

REM build DotCoreEditor
ECHO ��ʼ����DotCoreEdior >> %BuildLogPath%
devenv .\DotCore\DotCore.sln /rebuild "%BuildMode%" /project "DotCoreEditor" /out %BuildLogPath%

REM build DotContext
ECHO ��ʼ����DotContext >> %BuildLogPath%
devenv .\DotContext\DotContext.sln /rebuild "%BuildMode%" /project "DotContext" /out %BuildLogPath%

REM build DotAI
ECHO ��ʼ����DotAI >> %BuildLogPath%
devenv .\DotAI\DotAI.sln /rebuild "%BuildMode%" /project "DotAI" /out %BuildLogPath%

REM build DotCrypto
ECHO ��ʼ����DotCrypto >> %BuildLogPath%
devenv .\DotCrypto\DotCrypto.sln /rebuild "%BuildMode%" /project "DotCrypto" /out %BuildLogPath%

REM build DotTimer
ECHO ��ʼ����DotTimer >> %BuildLogPath%
devenv .\DotTimer\DotTimer.sln /rebuild "%BuildMode%" /project "DotTimer" /out %BuildLogPath%

REM build DotDispatch
ECHO ��ʼ����DotDispatch >> %BuildLogPath%
devenv .\DotDispatch\DotDispatch.sln /rebuild "%BuildMode%" /project "DotDispatch" /out %BuildLogPath%

REM build DotGOPool
ECHO ��ʼ����DotGOPool >> %BuildLogPath%
devenv .\DotGOPool\DotGOPool.sln /rebuild "%BuildMode%" /project "DotGOPool" /out %BuildLogPath%

REM build DotSnappy
ECHO ��ʼ����DotSnappy >> %BuildLogPath%
devenv .\DotSnappy\DotSnappy.sln /rebuild "%BuildMode%" /project "DotSnappy" /out %BuildLogPath%

REM build DotNet
ECHO ��ʼ����DotNet >> %BuildLogPath%
devenv .\DotNet\DotNet.sln /rebuild "%BuildMode%" /project "DotNet" /out %BuildLogPath%

PAUSE