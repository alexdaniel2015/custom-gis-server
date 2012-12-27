<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Ajax" %>

<%@ Register TagPrefix="smap" Namespace="AjaxMap" Assembly="AjaxMap" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <meta charset="utf-8">
    <title>Custom Gis Server</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="author" content="Vitaliy Zasadnyy, Yuriy Hoy, Roman Drebotiy, Andriy Mamchur, Oleh Bulatovskuy">

     <!-- Le styles -->
    <link href="bootstrap/css/bootstrap.css" rel="stylesheet">
    <style>
      body {
        padding-top: 60px; /* 60px to make the container go all the way to the bottom of the topbar */
      }
    </style>
    <link href="bootstrap/css/bootstrap-responsive.css" rel="stylesheet">

    <!-- HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->

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

    <div class="navbar navbar-inverse navbar-fixed-top">
      <div class="navbar-inner">
        <div class="container">
          <a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse">
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </a>
          <a class="brand" href="#">Custom Gis Server</a>
          <div class="nav-collapse collapse">
            <ul class="nav">
              <li class="active"><a href="#">Map</a></li>
              <li><a href="#about">Directory</a></li>
              <li><a href="#contact">About</a></li>
            </ul>
          </div><!--/.nav-collapse -->
        </div>
      </div>
    </div>

   

    <div class="container">
    <div id="testdiv"></div>
    <form id="Form1" runat="server" action="Default.aspx">
        <div class="row">           
            <div id="divLayers" class="span3 config-menu">
                <h4>Layers Configuration</h4>
                <asp:Panel ID="pnlLayers" runat="server" />
            </div>
            <div class="span7">
                
                 <div class="btn-toolbar">
                     <div id="clickTools" class="btn-group">
                         <div id="zoomIn" class="btn" onclick="ajaxMapObj.zoomAmount = 3; togToolbar(this.id,'clickTools');"><i class="icon-zoom-in"></i>  Zoom In</div>
                         <div id="zoomOut" class="btn" onclick="ajaxMapObj.zoomAmount = 0.33; togToolbar(this.id,'clickTools');"><i class="icon-zoom-out"></i>  Zoom Out</div>
                     </div>
                    <div id="dragTools" class="btn-group">
                         <div id="pan" class="btn" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');"><i class="icon-move"></i>  Pan</div>
                         <div id="boxZoom" class="btn" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');"><i class="icon-retweet"></i>  Zoom Area</div>
                        <div id="measure"class="btn" onclick="javascript:SharpMap_ToolSelect(ajaxMapObj,this.id); togToolbar(this.id,'dragTools');"><i class="icon-resize-horizontal"></i>  Measure</div>
                     </div>
                    <div id="tempTools" class="btn-group">
                         <div id="zoomExtents" class="btn" onclick="javascript:SharpMap_ZoomExtents(ajaxMapObj);"><i class="icon-fullscreen"></i>  Reset View</div>
                     </div>
                </div>

                 <smap:AjaxMapControl Width="100%" Height="655px" ID="ajaxMap" runat="server" />

            </div>
        </div>
        
    </form>
        

    </div> <!-- /container -->

     <div id="footer">
      <div class="container">
        <p class="muted credit"> &copy; 2012 <a href="http://zasadnyy.org.ua">Vitaliy Zasadnyy</a>, Yuriy Hoy, Roman Drebotiy, Andriy Mamchur and Oleh Bulatovskuy.</p>
      </div>
    </div>

    <asp:Label ID="Message" runat="server" />

     <!-- Le javascript
    ================================================== -->
    <!-- Placed at the end of the document so the pages load faster -->
    <script src="bootstrap/js/jquery.js"></script>
    <script src="bootstrap/js/bootstrap-transition.js"></script>
    <script src="bootstrap/js/bootstrap-alert.js"></script>
    <script src="bootstrap/js/bootstrap-modal.js"></script>
    <script src="bootstrap/js/bootstrap-dropdown.js"></script>
    <script src="bootstrap/js/bootstrap-scrollspy.js"></script>
    <script src="bootstrap/js/bootstrap-tab.js"></script>
    <script src="bootstrap/js/bootstrap-tooltip.js"></script>
    <script src="bootstrap/js/bootstrap-popover.js"></script>
    <script src="bootstrap/js/bootstrap-button.js"></script>
    <script src="bootstrap/js/bootstrap-collapse.js"></script>
    <script src="bootstrap/js/bootstrap-carousel.js"></script>
    <script src="bootstrap/js/bootstrap-typeahead.js"></script>
</body>
</html>
