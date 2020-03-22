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
ECHO 开始构建DotLog >> %BuildLogPath%
devenv .\DotLog\DotLog.sln /rebuild "%BuildMode%" /project "DotLog" /out %BuildLogPath%

REM build DotCore
ECHO 开始构建DotCore >> %BuildLogPath%
devenv .\DotCore\DotCore.sln /rebuild "%BuildMode%" /project "DotCore" /out %BuildLogPath%

REM build DotCoreEditor
ECHO 开始构建DotCoreEdior >> %BuildLogPath%
devenv .\DotCore\DotCore.sln /rebuild "%BuildMode%" /project "DotCoreEditor" /out %BuildLogPath%

REM build DotContext
ECHO 开始构建DotContext >> %BuildLogPath%
devenv .\DotContext\DotContext.sln /rebuild "%BuildMode%" /project "DotContext" /out %BuildLogPath%

REM build DotAI
ECHO 开始构建DotAI >> %BuildLogPath%
devenv .\DotAI\DotAI.sln /rebuild "%BuildMode%" /project "DotAI" /out %BuildLogPath%

REM build DotCrypto
ECHO 开始构建DotCrypto >> %BuildLogPath%
devenv .\DotCrypto\DotCrypto.sln /rebuild "%BuildMode%" /project "DotCrypto" /out %BuildLogPath%

REM build DotTimer
ECHO 开始构建DotTimer >> %BuildLogPath%
devenv .\DotTimer\DotTimer.sln /rebuild "%BuildMode%" /project "DotTimer" /out %BuildLogPath%

REM build DotDispatch
ECHO 开始构建DotDispatch >> %BuildLogPath%
devenv .\DotDispatch\DotDispatch.sln /rebuild "%BuildMode%" /project "DotDispatch" /out %BuildLogPath%

REM build DotGOPool
ECHO 开始构建DotGOPool >> %BuildLogPath%
devenv .\DotGOPool\DotGOPool.sln /rebuild "%BuildMode%" /project "DotGOPool" /out %BuildLogPath%

REM build DotSnappy
ECHO 开始构建DotSnappy >> %BuildLogPath%
devenv .\DotSnappy\DotSnappy.sln /rebuild "%BuildMode%" /project "DotSnappy" /out %BuildLogPath%

REM build DotNet
ECHO 开始构建DotNet >> %BuildLogPath%
devenv .\DotNet\DotNet.sln /rebuild "%BuildMode%" /project "DotNet" /out %BuildLogPath%

PAUSE