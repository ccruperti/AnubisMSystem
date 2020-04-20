/****** Script for SelectTopNRows command from SSMS  ******/
UPDATE [anubisdbms].[MON].[DataSensores]
SET [MON].[DataSensores].Error=0, [MON].[DataSensores].EncimaNormal=0, [MON].[DataSensores].Chequeado=0,[MON].[DataSensores].DebajoNormal=0, [MON].[DataSensores].Notificado=0
  
  UPDATE [anubisdbms].[MON].[DataSensores]
SET [MON].[DataSensores].FechaRegistro=GETDATE()
  