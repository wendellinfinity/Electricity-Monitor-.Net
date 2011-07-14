<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="ElectricityMonitor._Default" %>

<asp:content ID="HeaderContent" runat="server" 
     ContentPlaceHolderID="HeadContent">
</asp:content>
<asp:content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        Welcome to Prototype Labs!
    </h2>
    <p>
        Select the node from the left pane to view data. Sensor data refreshes every 1 minute.
    </p>
    <div class="grapharea" style="width:200px;float:left;">
        Sensor Nodes: <br />
        <asp:datalist id="nodeList" runat="server">
        <itemtemplate>
        <asp:linkbutton id="nodeLink" runat="server"></asp:linkbutton> 
        </itemtemplate>
        </asp:datalist>
    </div>
    <div class="grapharea" style="float:left;width:700px;">
        Sensor Graph: <br />
    </div>
</asp:content>
