@echo on
@echo *******************请以管理员身份运行此脚本***************************

@echo *******************重始EPMCSService服务***************************

net stop EPMCSService
net start EPMCSService

exit