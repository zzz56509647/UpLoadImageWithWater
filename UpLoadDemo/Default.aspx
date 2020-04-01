<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="UpLoadDemo._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <div>
            水印文字：<asp:TextBox ID="txtText" runat="server"></asp:TextBox>&nbsp;
            <asp:FileUpload ID="upLoad" runat="server" Width="250" Height="25" />&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnUpLoad" runat="server" Text="上传" Width="50" OnClick="btnUpLoad_Click" />&nbsp;H:<asp:TextBox
                ID="txtHeight" runat="server" Width="25" Text="100" MaxLength="4" />&nbsp;W:<asp:TextBox
                    ID="txtWidth" runat="server" Text="100" Width="25" MaxLength="4" />
        </div>
        <div id="divMessage" runat="server" style="color: Red">
        </div>
        <div id="divSource" runat="server" visible="false">
            <asp:Label ID="lblSource" runat="server" Text="原图" /><br />
            <asp:Image ID="imgSource" runat="server" />
        </div>
        <div id="divThumbnail" runat="server" visible="false">
            <asp:Label ID="lblThumbnail" runat="server" Text="缩略图" /><br />
            <asp:Image ID="imgThumbnail" runat="server" />
        </div>
        <div id="divWatermark" runat="server" visible="false">
            <asp:Label ID="Label1" runat="server" Text="文字水印" /><br />
            <asp:Image ID="imgWatermark" runat="server" />
        </div>
        <div id="divWatermarkPic" runat="server" visible="false">
            <asp:Label ID="Label2" runat="server" Text="图片水印" /><br />
            <asp:Image ID="imgWatermarkPic" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
