--创建语句必须要先删除后才能创建


--删除表
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[drp_test]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].drp_test
GO

--删除函数
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[f_drp_test]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].f_drp_test
GO

--删除视图
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[v_drp_test]') and OBJECTPROPERTY(id, N'IsView') = 1)
drop view [dbo].v_drp_test
GO

--删除存储过程
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[drp_test]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].drp_test

GO


