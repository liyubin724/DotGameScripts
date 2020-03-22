DotLog
    -> log4net

DotCore
    -> DotLog

DotCoreEditor
    -> DotCore
    -> DotLog
    -> ReflectionMagic

DotContext
    -> DotCore

DotAI
    -> DotContext

DotCrypto
    ->

DotTimer
    -> DotCore

DotDispatch
    -> DotCore
    -> DotLog
    -> DotTimer

DotGOPool
    ->DotCore
    ->DotLog
    ->DotTimer

DotNet
    -> DotCore
    -> DotCrypto
    -> DotLog
    -> DotSnappy
    -> Google.Protobuf
    -> Newtonsoft.Json

DotSnappy
    -> 