<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAuth.aspx.cs" Inherits="CombinedProject.UserAuth" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <style>
        body{
            height: 100%;
            margin: 0;
            background-color: #e0f7fa;
        }

        .heading {
            font-size: 30px;
        }

        .container {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100%;
        }

        #login {
            font-family: sans-serif;
            border: 1px #0094ff solid;
            border-radius: 10px;
            width: 500px;
            padding: 20px;
            background-color: #ffffff;
            text-align: center;
            margin-top: 30vh;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }

        #txtUserId, #txtPassword {
            border-radius: 10px;
            border: 1px solid;
            width: 50%;
            padding: 10px;
            margin: 10px;
        }

        #btnLogin {
            width: 50%;
            padding: 10px;
            border-radius: 10px;
            background-color: #0094ff;
            color: white;
            border: none;
            cursor: pointer;
            margin: 5px;
        }

        #btnLogin:hover {
            background-color: #007acc;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div id="login">
                <div class="heading">E-District Dashboard Login</div>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                <br />
                <!-- <asp:Label ID="lblUserId" runat="server" Text="User ID:"></asp:Label> -->
                <asp:TextBox ID="txtUserId" runat="server" AutoCompleteType="Disabled" placeholder="Enter User ID"></asp:TextBox>
                <br />
                <!-- <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label> -->
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                <br />
                <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" />
            </div>
        </div>
    </form>
</body>
</html>
