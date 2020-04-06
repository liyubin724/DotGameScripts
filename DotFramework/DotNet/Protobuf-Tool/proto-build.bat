@ECHO OFF

rem ProtoConfigGenerator.exe���ڵ�λ��
SET GeneratorPath=Generator\ProtoConfigGenerator.exe
rem protoc.exe���ڵ�λ��
SET ProtocPath=protoc-3.11.4-win64\bin\protoc.exe

rem ����������ļ�proto-config.xml
SET ProtoConfigPath=proto-config.xml
rem �ű�ģ�������ļ�
SET TemplateConfigPath=Generator\script-template\script_template.xml

rem proto�ļ��ĺ�׺
SET ProtoFileExt=.proto

rem ��ȡ����Ľű����Ŀ¼
SET ScriptOutputRootDir=%1

rem ��ȡ��������������(CSharp/Lua/CPlusPlus)
SET LanguageType=%2
IF /I "%LanguageType%"=="CSharp" (
    SET ScriptOutputFolderName=csharp
)
IF /I "%LanguageType%"=="Lua" (
    SET ScriptOutputFolderName=lua
)

rem ��ȡ�����ű�ʹ��Ŀ�꣬�Ƿ��������ͻ���(Client/Server)
SET PlatformType=%3
IF /I "%PlatformType%"=="Client" (
    SET ScriptOutputTargetFolderName=client
) else (
    SET ScriptOutputTargetFolderName=server
)

rem ���սű������Ŀ¼
SET ScriptOutputDir=%ScriptOutputRootDir%\%ScriptOutputTargetFolderName%\%ScriptOutputFolderName%
IF NOT EXIST %ScriptOutputDir% (
    MD %ScriptOutputDir%
)

rem ��ȡproto�ļ����ڵ�Ŀ¼
SET ProtoInputFileDir=%4

IF /I "%PlatformType%"=="Client" (
    ECHO ----��ʼ���� -�ͻ���- ������Ϣ�������ű��ļ�----
) else (
    ECHO ----��ʼ���� -������- ������Ϣ�������ű��ļ�----
)

rem ����ProtoConfigGenerator������Ϣ�������ű�
%GeneratorPath% -c %ProtoConfigPath% -t %TemplateConfigPath% -o %ScriptOutputDir% -l %LanguageType% -p %PlatformType%

IF /I "%PlatformType%"=="Client" (
    ECHO ----ת��Proto�ļ�Ϊ  -�ͻ���-  �ű�----
) ELSE (
    ECHO ----ת��Proto�ļ�Ϊ  -������-  �ű�-----
)

FOR /R %%f IN (%ProtoInputFileDir%\*%ProtoFileExt%) do (
    IF /I "%LanguageType%"=="CSharp" (
        %ProtocPath% --csharp_out=%ScriptOutputDir% --proto_path=%ProtoInputFileDir% %%~nxf
    )
)

ECHO ----�������----