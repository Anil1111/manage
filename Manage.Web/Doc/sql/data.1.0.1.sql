-- Sys_qxmx.typ 2��̨

-- û��Ȩ��
IF NOT EXISTS(SELECT * FROM Sys_qxmx WHERE typ=2 AND action='/Home' AND method='Login')
BEGIN
    INSERT INTO Sys_qxmx (action, method, sort, remark, mkid, typ) VALUES ('/Home', 'Login', 5, '��¼', '0', 2)
END

-- ƽ̨��Ѷ
IF NOT EXISTS(SELECT * FROM Sys_xtmk WHERE mid='B0100')
BEGIN
    INSERT INTO Sys_xtmk (mid, mname, typ, sort, state, iconCls) VALUES ('B0100', '��Ѷģ��', 2, 5, 1, 'fa fa-list-alt')
END

IF NOT EXISTS(SELECT * FROM Sys_qxsz WHERE mid='B01000010')
BEGIN
    INSERT INTO Sys_qxsz (mkid, mid, mname, name, url, state, sort) VALUES ('B0100', 'B01000010', 'ƽ̨��Ѷ', '��Ѷģ��', '/PlatformInfo/PlatformInfoList', 1, 5)
END

IF NOT EXISTS(SELECT * FROM Sys_qxmx WHERE typ=2 AND action='/PlatformInfo' AND method='PlatformInfoList')
BEGIN
    INSERT INTO Sys_qxmx (action, method, sort, remark, mkid, typ) VALUES ('/PlatformInfo', 'PlatformInfoList', 10, 'ƽ̨��Ѷ', 'B01000010', 2)
END