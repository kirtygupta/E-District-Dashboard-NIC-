<!-- Developed by [ Nikhil Kumar Vashist Kirty Gupta, Intern - National Informatics Centre ] -->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CombinedProject.Default" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>E-District Dashboard</title>
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <style>
        body {
            background-color: #e0f7fa;
        }

        .container {
            display: flex;
            flex-direction: column;
            align-items: center;
        }

        .dropdown-container {
            display: flex;
            justify-content: center;
            gap: 5px;
        }

        #ddlServices {
            max-width: 55%; /* or set a specific value like 300px */
            width: auto;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            padding: 5px;
            border-radius: 10px;
            border: 1px solid black;
            font-family: sans-serif;
        }

        #ddlDistricts, #ddlSubdivisions {
            width: 115px;
            padding: 5px;
            border-radius: 10px;
            border: 1px solid black;
        }

        #ddlServices option:hover {
            overflow: visible;
            white-space: normal;
            word-wrap: break-word;
        }

        .chart-container {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-around;
        }

        .chart-heading {
            position: absolute;
            top: 5px;
            left: 50%;
            transform: translateX(-50%);
            font-size: 16px;
            font-weight: bold;
            color: black;
            font-family: sans-serif;
            font-weight: 100;
            font-size: 12px;
            color: #222222;
        }

        .chart-box {
            position: relative;
            background-color: #ffffff;
            width: 40vw;
            height: 40vh;
            border: solid 1px green;
            border-radius: 10px;
            display: flex;
            justify-content: center;
            align-items: center;
            margin-bottom: 10px;
        }

        .logout-container {
            position: absolute;
            top: 10px;
            right: 10px;
        }

        #btnLogout {
            background-color: #EE3333;
            color: white;
            border: none;
            padding: 10px;
            border-radius: 5px;
            cursor: pointer;
        }

        #btnLogout:hover {
            background-color: #B33;
        }
    </style>
</head>
<body>
    <h2 style="text-align: center; font-family: sans-serif; color: #0000A0">E-District Services Dashboard</h2>
    <form id="form1" runat="server">
        <div class="logout-container">
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
        </div>
        <div class="container">
            <div class="dropdown-container" style="margin-bottom: 10px;">
                <asp:DropDownList ID="ddlServices" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlServices_SelectedIndexChanged"></asp:DropDownList>
                <asp:DropDownList ID="ddlDistricts" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDistricts_SelectedIndexChanged"></asp:DropDownList>
                <asp:DropDownList ID="ddlSubdivisions" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubdivisions_SelectedIndexChanged"></asp:DropDownList>
            </div>
            <div class="chart-container">
                <div class="chart-box">
                    <div class="chart-heading">Gender Wise Distribution of Applicants</div>
                    <canvas id="pieChart" style="position: center;"></canvas>
                </div>
                <div class="chart-box">
                    <canvas id="lineChart1"></canvas>
                </div>
                <div class="chart-box">
                    <canvas id="lineChart2"></canvas>
                </div>
                <div class="chart-box">
                    <canvas id="barChart"></canvas>
                </div>
            </div>
        </div>
    </form>
    <script>
        // Chart initialization script will go here
    </script>
</body>
</html>
