@ECHO OFF

call .\proto-build.bat output CSharp Client protos
call .\proto-build.bat output CSharp Server protos

PAUSE