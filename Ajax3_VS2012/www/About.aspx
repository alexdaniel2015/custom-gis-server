<%--<%@ Page Title="Title" Language="C#" MasterPageFile="Site.master" %>--%>
<%@ Page Language="C#" AutoEventWireup="true"  %>

<%@ Register TagPrefix="smap" Namespace="AjaxMap" Assembly="AjaxMap" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <meta charset="utf-8">
    <title>About|Custom Gis Server</title>
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
          <a class="brand" href="/www/">Custom Gis Server</a>
          <div class="nav-collapse collapse">
            <ul class="nav">
              <li ><a href="/www/">Map</a></li>
              <li><a href="#directory">Directory</a></li>
              <li class="active"><a href="/www/About.aspx">About</a></li>
            </ul>
          </div><!--/.nav-collapse -->
        </div>
      </div>
    </div>

     

    <div class="container">

        <div class="page-header">
            <h1>Our team</h1>
        </div>

        <div class="row-fluid">

            <ul class="thumbnails">

                <li class="span4">
                <div class="thumbnail">
                  <img data-src="holder.js/300x200" alt="Hoy Yuriy" src="images/fb_default_profile.jpg">
                  <div class="caption">
                    <h3>Hoy Yuriy</h3>
                    <p>core functionality, solution architecture, management, service layer</p>
                    <p><a href="#" class="btn">Incognito</a></p>
                  </div>
                </div>
              </li>

              <li class="span4">
                <div class="thumbnail">
                  <img data-src="holder.js/300x200" alt="Vitaliy Zasadnyy" src="images/zasadnyy.JPG">
                  <div class="caption">
                    <h3>Vitaliy Zasadnyy</h3>
                    <p>git repository, page styles, map styles, core functionality</p>
                    <p><a href="http://zasadnyy.org.ua/" class="btn btn-primary">Site</a> <a href="https://plus.google.com/102963345585524280596/about" class="btn btn-danger">Google+</a></p>
                  </div>
                </div>
              </li>
              
              <li class="span4">
                <div class="thumbnail">
                  <img data-src="holder.js/300x200" alt="Roman Drebotiy" src="images/drebotiy.JPG">
                  <div class="caption">
                    <h3>Roman Drebotiy</h3>
                    <p>database management and architecture, map data, map layers</p>
                    <p><a href="http://vk.com/id9760054" class="btn btn-info">VK</a></p>
                  </div>
                </div>
              </li>
            </ul>
        </div>

        <div class="row-fluid">
            <ul class="thumbnails">
              <li class="span4">
                <div class="thumbnail">
                  <img data-src="holder.js/300x200" alt="Andriy Mamchur" src="images/mamch.jpg">
                  <div class="caption">
                    <h3>Andriy Mamchur</h3>
                    <p>core functionality, support, testing</p>
                    <p><a href="http://vk.com/id42152921" class="btn btn-info">VK</a></p>
                  </div>
                </div>
              </li>
              <li class="span4">
                <div class="thumbnail">
                  <img data-src="holder.js/300x200" alt="Oleh Bulatovskuy" src="images/bulat.jpg">
                  <div class="caption">
                    <h3>Oleh Bulatovskuy</h3>
                    <p>core functionality, layers filters</p>
                    <p><a href="http://vk.com/gelo_one" class="btn btn-info">VK</a></p>
                  </div>
                </div>
              </li>
            
            </ul>
          </div>
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
    <script src="bootstrap/js/holder.js"></script>
</body>
</html>
