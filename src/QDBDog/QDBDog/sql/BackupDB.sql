﻿
USE master;
print  @@SERVERNAME 
print 'user '+SYSTEM_USER+';';
go


IF EXISTS(SELECT 1 FROM sys.certificates WHERE name='AutoBakCert')
BEGIN
	BACKUP DATABASE [@dbname@] TO  DISK = N'@path@.dll' WITH RETAINDAYS = 30, NOFORMAT, INIT,  NAME = N'@dbname@自动完整备份', SKIP, NOREWIND, NOUNLOAD,COMPRESSION,   STATS = 10 ,ENCRYPTION (ALGORITHM = AES_256, SERVER CERTIFICATE = [AutoBakCert]);
END 
else
PRINT N'请先安装备份加密证书AutoBakCert'
go
