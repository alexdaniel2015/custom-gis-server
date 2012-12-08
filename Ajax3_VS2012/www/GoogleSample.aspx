<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GoogleSample.aspx.cs" Inherits="GoogleSample" %>

<%@ Register assembly="AjaxMap" namespace="AjaxMap" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="GoogleForm" runat="server">
    <div style="height: 249px; width: 550px">
    
        <cc1:AjaxMapControl ID="AjaxMapControl" runat="server" 
            style="top: 0px; left: 0px; height: 219px; width: 544px" />
    
    </div>
    </form>
</body>
</html>
