using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BruTile.Web;
using GeoAPI.Geometries;

public partial class GoogleSample : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxMapControl.Map = MapHelper.InitializeGoogleMap(GoogleMapType.GoogleMap);
        AjaxMapControl.FadeSpeed = 5;
        AjaxMapControl.ZoomSpeed = 5;
        Envelope mapExtents = AjaxMapControl.Map.GetExtents();
        AjaxMapControl.Map.Zoom = mapExtents.Width;
        AjaxMapControl.Map.Center = mapExtents.Centre;
        AjaxMapControl.ResponseFormat = "googlehandler.aspx?Width=[WIDTH]&Height=[HEIGHT]&Zoom=[ZOOM]&X=[X]&Y=[Y]";
        // ajaxMap.OnClickEvent = map_Click();
        //createCheckboxes(ajaxMap.Map.Layers);
    }
}