@echo on
@echo *******************���Թ���Ա������д˽ű�***************************

set binPath=%CD%

echo "%binPath%\ReStartServer.exe"

@echo ���ڰ�װ...(sc��ʽҪ��,=��ǰ�����пո�,����Ҫ�пո�)
sc create ReStartServer binPath= "%binPath%\ReStartServer.exe" displayname= "ReStartServer" start= "auto"

sc description ReStartServer "���ڿͻ����ܱ����ݲɼ�������ʼ,����˷��񱻽��ã����޷��ɼ����ݡ�" 

@echo ��װ���!  start= "auto"
@echo ����װλ��: %binPath%
@echo �������´�����ϵͳ���Զ�����
@echo   ���� 
@echo ʹ������:  net start ReStartServer    �ֹ���������
@echo ʹ������:  sc delete ReStartServer ж�ط���
@echo .
@echo .
@pause