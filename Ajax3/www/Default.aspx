<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Ajax" %>

<%@ Register TagPrefix="smap" Namespace="AjaxMap" Assembly="AjaxMap" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Simple Example</title>
    <link rel="stylesheet" href="styles/main.css">
</head>
<body>
    <form id="Form1" runat="server" action="Default.aspx">
    <div style="text-align: center; padding: 1em;">
        <h2>
            Map</h2>
        <div style="background: #f3f2e7; color: #000; width: 74em; text-align: left; height: 54em;
            border: solid 1px #000">
            <div style="position: relative; margin-left: 18em; width: 52em;">
                <div id="clickTools" class="toolbar" style="width: 4.2em">
                    <img id="zoomIn" src="images/zoom_in.gif" onclick="ajaxMapObj.zoomAmount = 3; togToolbar(this.id,'clickTools');"
                        onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';"
                        class="pressed" />
                    <img id="zoomOut" src="images/zoom_out.gif" onclick="ajaxMapObj.zoomAmount = 0.33; togToolbar(this.id,'clickTools');"
                        onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';"
                        class="button" />
                </div>
                <!--<div id="dragTools" class="toolbar" style="width:6.5em;left:6em;">
                <img id="pan" src="images/pan.gif" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');" onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';" class="pressed" />
                <img id="boxZoom" src="images/zoom_selected.gif" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');" onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';" class="button" />
                <img id="measure" src="images/measure.gif" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');" onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';" class="button" />
            </div>-->
                <div id="tempTools" class="toolbar" style="width:2em;left:17em;">
           <!--     <img id="zoomExtents" src="images/zoom_full.gif" onclick="javascript:SharpMap_ZoomExtents(ajaxMapObj);" onmouseover="this.className='raised';" onmouseout="this.className='button';" onmouseup="this.className='raised';" onmousedown="this.className='pressed';" class="button" />-->
            </div>
            </div>
            <div id="divLayers" style="margin:2.5em 1em 0em 1em;width:15em;position:absolute;background:#fff;border:solid 1px #000;padding:0.5em">
             <!--<h4>Layers</h4>-->
           <!-- <asp:Panel ID="pnlLayers" runat="server" Height="255px" Width="191px" />-->
        </div>
            <div style="position: absolute; width: 52em; height: 48em; margin-top: 2em; margin-left: 18em;
                border: 1px solid #000">
                <smap:AjaxMapControl Width="52em" Height="48em" ID="ajaxMap" runat="server" />
            </div>
        </div>
    </div>
    </form>
    <asp:Label ID="Message" runat="server" />
</body>
</html>
