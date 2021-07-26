<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResetPassword.aspx.cs" Inherits="MLPPages.ResetPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reset Password</title>
    <link href="css/style.css" rel="stylesheet" />
    <link href='https://fonts.googleapis.com/css?family=Source+Sans+Pro' rel='stylesheet'
        type='text/css' />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
</head>
<body>
    <div style="background-color: White;">
        <img src="images/MobilNoBG.jpg" style="margin: 10px; width: auto; height: 50px;" alt="" />
    </div>
    <form id="form1" runat="server">
    <div style="background-color: White;">
        <h4>
            Reset Password
        </h4>
    </div>
    <asp:TextBox ID="PasswordTxt" TextMode="Password" runat="server" class="pw" placeholder="Enter Password"></asp:TextBox>
    <div style="text-align: left;">
        <asp:RequiredFieldValidator ID="ReqV1" runat="server" Display="Dynamic" ControlToValidate="PasswordTxt"
            Text="Required" ForeColor="Red" Font-Bold="true" Font-Size="Medium" ValidationGroup="ResetPassword"></asp:RequiredFieldValidator>
    </div>
    <asp:TextBox ID="RePasswordTxt" TextMode="Password" runat="server" class="pw" placeholder="Re-enter Password"></asp:TextBox>
    <div style="text-align: left;">
        <asp:RequiredFieldValidator ID="ReqV2" runat="server" Display="Dynamic" ControlToValidate="RePasswordTxt"
            Text="Required" ForeColor="red" Font-Bold="true" Font-Size="Medium" ValidationGroup="ResetPassword"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="comparePasswords" runat="server" ControlToCompare="PasswordTxt"
            ControlToValidate="RePasswordTxt" ErrorMessage="passwords do not match up!" Display="Dynamic"
            Font-Size="Medium" Font-Bold="true" ForeColor="#7f8c8d" ValidationGroup="ResetPassword" />
    </div>
    <asp:Button ID="ResetBtn" runat="server" CssClass="button" Text="Reset" Style="cursor: pointer"
        ValidationGroup="ResetPassword" OnClick="ResetBtn_Click" />
    <asp:Label ID="SuccessMsg" runat="server" Text="The password has been changed successfully... "
        ForeColor="#7f8c8d" Font-Bold="true" Font-Size="Medium" Visible="false"></asp:Label>
    </form>
</body>
</html>
