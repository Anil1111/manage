-- Sys_qxmx.typ 2后台

-- 没有权限
IF NOT EXISTS(SELECT * FROM Sys_qxmx WHERE typ=2 AND action='/Home' AND method='Login')
BEGIN
    INSERT INTO Sys_qxmx (action, method, sort, remark, mkid, typ) VALUES ('/Home', 'Login', 5, '登录', '0', 2)
END

-- 平台资讯
IF NOT EXISTS(SELECT * FROM Sys_xtmk WHERE mid='B0100')
BEGIN
    INSERT INTO Sys_xtmk (mid, mname, typ, sort, state, iconCls) VALUES ('B0100', '资讯模块', 2, 5, 1, 'fa fa-list-alt')
END

IF NOT EXISTS(SELECT * FROM Sys_qxsz WHERE mid='B01000010')
BEGIN
    INSERT INTO Sys_qxsz (mkid, mid, mname, name, url, state, sort) VALUES ('B0100', 'B01000010', '平台资讯', '资讯模块', '/PlatformInfo/PlatformInfoList', 1, 5)
END

IF NOT EXISTS(SELECT * FROM Sys_qxmx WHERE typ=2 AND action='/PlatformInfo' AND method='PlatformInfoList')
BEGIN
    INSERT INTO Sys_qxmx (action, method, sort, remark, mkid, typ) VALUES ('/PlatformInfo', 'PlatformInfoList', 10, '平台资讯', 'B01000010', 2)
END