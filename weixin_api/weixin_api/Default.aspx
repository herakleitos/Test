<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="weixin_api.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>公众号API测试</title>
    <style type="text/css">
        #Text1 {
            height: 254px;
            width: 517px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="tbMenuContent" runat="server" Height="262px" Width="479px">请输入JSON格式菜单内容。。。</asp:TextBox>
            <asp:TextBox ID="tbResult" runat="server" Height="262px" Width="479px">操作结果</asp:TextBox>
        </div>
        <p>
            <asp:Button ID="btnCreateMenu" runat="server" OnClick="btnCreateMenu_Click" Text="创建菜单" />
            <asp:Button ID="btnDeleteMenu" runat="server" OnClick="btnDeleteMenu_Click" Text="删除菜单" />
        </p>
    </form>
</body>
</html>
