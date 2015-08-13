@echo on
@echo *******************请以管理员身份运行此脚本***************************

set binPath=%CD%

echo "%binPath%\ReStartServer.exe"

@echo 正在安装...(sc格式要求,=号前不能有空格,后面要有空格)
sc create ReStartServer binPath= "%binPath%\ReStartServer.exe" displayname= "ReStartServer" start= "auto"

sc description ReStartServer "用于客户电能表数据采集服务重始,如果此服务被禁用，将无法采集数据。" 

@echo 安装完成!  start= "auto"
@echo 服务安装位置: %binPath%
@echo 服务在下次重启系统后自动启动
@echo   或者 
@echo 使用命令:  net start ReStartServer    手工启动服务
@echo 使用命令:  sc delete ReStartServer 卸载服务
@echo .
@echo .
@pause