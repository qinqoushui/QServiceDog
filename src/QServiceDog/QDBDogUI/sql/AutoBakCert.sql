﻿USE master;
CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'ZyDBBak20200408';
go
USE master;
CREATE CERTIFICATE AutoBakCert FROM FILE = '{0}\AutoBakCert.cer'
WITH PRIVATE KEY (  FILE = '{0}\AutoBakCert.pkey', DECRYPTION  BY PASSWORD ='Cert123');
go
