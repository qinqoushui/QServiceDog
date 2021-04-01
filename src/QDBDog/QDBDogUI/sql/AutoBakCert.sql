USE master;
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'ZyDBBak20200408';
go
 
CREATE CERTIFICATE AutoBakCert FROM FILE = N'{0}\AutoBakCert.cer'
WITH PRIVATE KEY (  FILE = N'{0}\AutoBakCert.pkey', DECRYPTION  BY PASSWORD ='Cert123');
go

print N'启用证书和主控密钥完成';
go 