<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Ajax" %>

<%@ Register TagPrefix="smap" Namespace="AjaxMap" Assembly="AjaxMap" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title>Map</title>
    <link rel="stylesheet" href="styles/main.css" />

    <script type="text/javascript">

        var result;
        function test(ajaxMapObj) {
            result = null;
            FindElement(document, "ColorPicker1");
            var colors = "";

            for (var i = 0; i < result.length; i++) {
                colors += result[i].attributes["tag"].value + "=" + result[i].value + "|";
            }

            result = null;
            FindElement(document, "ColorPicker2");
            var colorsLine = "";

            for (var i = 0; i < result.length; i++) {
                colorsLine += result[i].attributes["tag"].value + "=" + result[i].value + "|";
            }

            //alert(ajaxMapObj);
            var part = ajaxMapObj.hiddenLayers.split('&');
            ajaxMapObj.hiddenLayers = part[0] + "&Colors=" + colors + "&ColorsLine=" + colorsLine;
            SharpMap_ZoomExtents(ajaxMapObj);
            //ajaxMapObj.LayersColorFill = "&Colors=" + colors;
            //ajaxMapObj.LayersColorLine = "&ColorsLine=" + colorsLine;
            //alert(ajaxMapObj.LayersColor);
 //           SharpMap_ZoomExtents(ajaxMapObj);

            //var src = ajaxMapObj.map1.src;
            //SetVars_ajaxMap();
            //alert("here");
            //ajaxMapObj.map1.src = ajaxMapObj.map1.src + "&colors=" + colors + "&colorsLine=" + colorsLine;

            //SetVars_ajaxMap();
            //alert(ajaxMapObj.map1.src);

            //
            //var map1 = document.getElementById("ajaxMap_ctl00");
            //var map2 = document.getElementById("ajaxMap_ctl01");
            //map1.src = src;
            //map2.src = "&colors=" + colors + "&colorsLine=" + colorsLine;
            //src = "&colors=" + colors + "&colorsLine=" + colorsLine;


    //        ajaxMapObj.map1.src += "&colors=" + colors + "&colorsLine=" + colorsLine;

     //       ajaxMapObj.map1.src += "&colors=" + colors + "&colorsLine=" + colorsLine;

    //       setTimeout(function () {
    //            
            //        }, 1000);

  //          
            //WebForm_InitCallback();
           // __doPostback("ajaxMap", "");

     //       __doPostback("Form1", "");      
     //       __doPostback("Form1", "");
     //       alert("here");
            //SharpMap_ZoomExtents(ajaxMapObj);
            
            //
            //WebForm_InitCallback();
            //

            //ajaxMapObj.map1.src = "maphandler.aspx?MAP=SimpleWorld&Width=1088&Height=640&Zoom=302378.87&X=101883.965&Y=114944.445&Layers=none";
            //WebForm_InitCallback();

            /*
            var xmlhttp = new XMLHttpRequest();
            xmlhttp.onreadystatechange = function () {
                if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {

                    //document.getElementById("testdiv").innerHTML = xmlhttp.responseText;
                }
            };

            xmlhttp.open("GET", ajaxMapObj.map1.src, true);
            xmlhttp.send();*/
        }


        function FindElement(element, id) {

            if (result == null) {
                result = [];
            }

            for (var i = 0; i < element.childNodes.length; i++) {
                var child = element.childNodes[i];
                if (child.tagName) {
                    //if id string is part of a control id
                    var idx = child.id.indexOf(id);
                    if (-1 != idx) {
                        result.push(child);
                        //break;
                    }
                    else {
                        FindElement(child, id);
                    }
                }
            }
        }
    </script>
</head>

<body">
    <div id="testdiv"></div>
    <form id="Form1" runat="server" action="Default.aspx">
        <div style="text-align: center; padding: 1em; background-color: gray;">
            <div style="background: #f3f2e7; color: #000; width: 100%; text-align: left; height: 54em; border: solid 1px #000">

                <div style="position: relative; margin-left: 350px; width: 52em;">

                    <div id="clickTools" class="toolbar" style="width: 4.2em">

                        <img id="zoomIn" src="images/zoom_in.gif" onclick="ajaxMapObj.zoomAmount = 3; togToolbar(this.id,'clickTools');"
                            onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';"
                            class="pressed" />

                        <img id="zoomOut" src="images/zoom_out.gif" onclick="ajaxMapObj.zoomAmount = 0.33; togToolbar(this.id,'clickTools');"
                            onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';"
                            class="button" />
                    </div>

                    <div id="dragTools" class="toolbar" style="width: 6.5em; left: 6em;">

                        <img id="pan" src="images/pan.gif" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');" onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';" class="pressed" />

                        <img id="boxZoom" src="images/zoom_selected.gif" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');" onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';" class="button" />

                        <img id="measure" src="images/measure.gif" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');" onmouseover="if(this.className=='button')this.className='raised';" onmouseout="if(this.className=='raised')this.className='button';" class="button" />

                    </div>

                    <div id="tempTools" class="toolbar" style="width: 2em; left: 17em;">

                        <img id="zoomExtents" src="images/zoom_full.gif" onclick="javascript:SharpMap_ZoomExtents(ajaxMapObj);" onmouseover="this.className='raised';" onmouseout="this.className='button';" onmouseup="this.className='raised';" onmousedown="this.className='pressed';" class="button" />
                    </div>

                </div>

                <div id="divLayers" style="margin: 25px 1em 0em 1em; width: 25em; height: 50em; position: absolute; background: #fff; border: solid 1px #000; padding: 0.5em">
                    <h4>Layers</h4>

                    <asp:Panel ID="pnlLayers" runat="server" Height="255px" Width="191px" />

                </div>

                <div style="position: absolute; width: 75em; height: 655px; margin-top: 2em; margin-left: 350px; border: 1px solid #000">

                    <smap:AjaxMapControl Width="75em" Height="655px" ID="ajaxMap" runat="server" />

                </div>
            </div>
        </div>
    </form>
    <asp:Label ID="Message" runat="server" />
</body>
</html>
