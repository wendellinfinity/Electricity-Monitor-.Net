<%@ Page Title="About Us" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="ElectricityMonitor.About" %>

<asp:content ID="HeaderContent" runat="server" 
     ContentPlaceHolderID="HeadContent">
</asp:content>
<asp:content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <p>
        This is a prototype solution that tries to have a detailed measure of electricity 
         consumption up to the project level. This prototype is currently only serving 
         per-sensor data for proof-of-concept purposes.</p>
</asp:content>
