@ECHO OFF

rem ProtoConfigGenerator.exe所在的位置
SET GeneratorPath=Generator\ProtoConfigGenerator.exe
rem protoc.exe所在的位置
SET ProtocPath=protoc-3.11.4-win64\bin\protoc.exe

rem 输入的配置文件proto-config.xml
SET ProtoConfigPath=proto-config.xml
rem 脚本模板配置文件
SET TemplateConfigPath=Generator\script-template\script_template.xml

rem proto文件的后缀
SET ProtoFileExt=.proto

rem 获取输入的脚本输出目录
SET ScriptOutputRootDir=%1

rem 获取导出的语言类型(CSharp/Lua/CPlusPlus)
SET LanguageType=%2
IF /I "%LanguageType%"=="CSharp" (
    SET ScriptOutputFolderName=csharp
)
IF /I "%LanguageType%"=="Lua" (
    SET ScriptOutputFolderName=lua
)

rem 获取导出脚本使用目标，是服务器还客户端(Client/Server)
SET PlatformType=%3
IF /I "%PlatformType%"=="Client" (
    SET ScriptOutputTargetFolderName=client
) else (
    SET ScriptOutputTargetFolderName=server
)

rem 最终脚本的输出目录
SET ScriptOutputDir=%ScriptOutputRootDir%\%ScriptOutputTargetFolderName%\%ScriptOutputFolderName%
IF NOT EXIST %ScriptOutputDir% (
    MD %ScriptOutputDir%
)

rem 获取proto文件所在的目录
SET ProtoInputFileDir=%4

IF /I "%PlatformType%"=="Client" (
    ECHO ----开始生成 -客户端- 网络消息的描述脚本文件----
) else (
    ECHO ----开始生成 -服务器- 网络消息的描述脚本文件----
)

rem 调用ProtoConfigGenerator生成消息的描述脚本
%GeneratorPath% -c %ProtoConfigPath% -t %TemplateConfigPath% -o %ScriptOutputDir% -l %LanguageType% -p %PlatformType%

IF /I "%PlatformType%"=="Client" (
    ECHO ----转换Proto文件为  -客户端-  脚本----
) ELSE (
    ECHO ----转换Proto文件为  -服务器-  脚本-----
)

FOR /R %%f IN (%ProtoInputFileDir%\*%ProtoFileExt%) do (
    IF /I "%LanguageType%"=="CSharp" (
        %ProtocPath% --csharp_out=%ScriptOutputDir% --proto_path=%ProtoInputFileDir% %%~nxf
    )
)

ECHO ----处理完成----