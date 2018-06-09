-- Éý¼¶sql

if exists(select * from syscolumns where id=object_id('dbo.News') and name='updateDate') 
begin
	ALTER TABLE [dbo].[News] DROP COLUMN [updateDate];
end