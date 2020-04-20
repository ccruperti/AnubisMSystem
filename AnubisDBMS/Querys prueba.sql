/****** Script for SelectTopNRows command from SSMS  ******/
SELECT COUNT(*)
  FROM [anubisdbms].[MON].[DataSensores]

 DELETE
  FROM [anubisdbms].[MON].[DataSensores]
  WHERE DataSensores.IdDataSensor>300