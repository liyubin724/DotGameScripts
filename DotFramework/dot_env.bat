ECHO OFF

rem 设置VS的目录，编译脚本时将会使用devenv进行编译
SET VS_DEVENV_DIR=C:\"Program Files (x86)"\"Microsoft Visual Studio"\2019\Enterprise\Common7\IDE

rem 编译的DLL针对的平台，可选项：Unity/Windows
SET DLL_PLATFORM=Unity

rem 如果设定的DLL_PLATFORM为Unity的话，可以二次指定针对的平台，可选为:WinEditor,Win,Android,iOS,All
SET UNITY_PLATFORM=WinEditor

rem 用于指定生成的DLL存放的目录，执行脚本时会清空此目录
SET DLL_OUTPUT_DIR=%~dp0DotLibs

rem 如果指定的DLL_OUTPUT_DIR已经存在则会完全删除后，再重新创建
if exist %DLL_OUTPUT_DIR% (
    RD /q /s %DLL_OUTPUT_DIR%
)
MD %DLL_OUTPUT_DIR%
