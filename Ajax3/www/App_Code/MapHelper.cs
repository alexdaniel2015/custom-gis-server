using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Drawing2D;

/// <summary>
/// Summary description for CreateMap
/// </summary>
public class MapHelper
{
    public static SharpMap.Map InitializeMap(Size size)
    {
			HttpContext.Current.Trace.Write("Initializing map...");
				
			//Initialize a new map of size 'imagesize'
			SharpMap.Map map = new SharpMap.Map(size);

            //Set up the boundaries layer
            SharpMap.Layers.VectorLayer layWA_Bounds = new SharpMap.Layers.VectorLayer("Washington State");
            layWA_Bounds.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\wa_bndry.shp"), true);
            //layWA_Bounds.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\Lviv\regions.shp"), true);
            Pen WAPen = new Pen(Color.FromArgb(255,0,0,1));
            layWA_Bounds.Style.Outline = WAPen;
            layWA_Bounds.Style.Fill = new SolidBrush(Color.FromArgb(255, 234, 234, 234));
            layWA_Bounds.Style.EnableOutline = true;

            //Set up Streams layer
            SharpMap.Layers.VectorLayer layHydromjr = new SharpMap.Layers.VectorLayer("Streams");
            layHydromjr.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\wa_hydro.shp"), true);
            layHydromjr.Style.Line = new Pen(Color.FromArgb(255, 0, 100, 255));
            layHydromjr.MaxVisible = 70000;
 
            //Set up National Park  area layer
            SharpMap.Layers.VectorLayer layWA_ParkAreas = new SharpMap.Layers.VectorLayer("Park Area");
            layWA_ParkAreas.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\natpark.shp"), true);
            layWA_ParkAreas.Style.Outline = new Pen(Color.FromArgb(200, 111, 161, 113));
            layWA_ParkAreas.Style.Fill = new SolidBrush(Color.FromArgb(200, 169, 219, 182));
            layWA_ParkAreas.Style.EnableOutline = true;

            //Set up a Park label layer
            SharpMap.Layers.LabelLayer layParkLabels = new SharpMap.Layers.LabelLayer("Park Labels");
            layParkLabels.DataSource = layWA_ParkAreas.DataSource;
            layParkLabels.Enabled = true;
            layParkLabels.LabelColumn = "NAME";
            layParkLabels.Style = new SharpMap.Styles.LabelStyle();
            layParkLabels.Style.ForeColor = Color.Navy;
            layParkLabels.Style.Font = new Font("Arial", 14, FontStyle.Bold);
            layParkLabels.Style.BackColor = new System.Drawing.SolidBrush(Color.FromArgb(75, 255, 255, 255));
            layParkLabels.MaxVisible = 130000;
            layParkLabels.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Center;
  
            //Set up National Forest layer
            SharpMap.Layers.VectorLayer layWA_Forests = new SharpMap.Layers.VectorLayer("National Forests");
            layWA_Forests.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\natfor.shp"), true);
            layWA_Forests.Style.Outline = new Pen(Color.FromArgb(200, 165, 165, 73));
            layWA_Forests.Style.Fill = new SolidBrush(Color.FromArgb(200, 236, 241, 185));
            layWA_Forests.Style.EnableOutline = true;

            //Set up National Rec layer
            SharpMap.Layers.VectorLayer layWA_RecArea = new SharpMap.Layers.VectorLayer("National Rec Areas");
            layWA_RecArea.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\natrec.shp"), true);
            layWA_RecArea.Style.Outline = new Pen(Color.FromArgb(200, 170, 116, 56));
            layWA_RecArea.Style.Fill = new SolidBrush(Color.FromArgb(200, 226, 198, 168));
            layWA_RecArea.Style.EnableOutline = true;

            //Set up National Parks layer group
            SharpMap.Layers.LayerGroup layWA_Parks = new SharpMap.Layers.LayerGroup("National Parks");
            layWA_Parks.Layers.Add(layWA_ParkAreas);
            layWA_Parks.Layers.Add(layParkLabels);

            //Set up Public Lands layer group
            SharpMap.Layers.LayerGroup layPublic = new SharpMap.Layers.LayerGroup("Public Lands");

 //           layPublic.Layers.Add(layWA_Parks); //THIS LAYER IS THE PROBELM

            layPublic.Layers.Add(layWA_Forests);
            layPublic.Layers.Add(layWA_RecArea);

            //Add layers to map
            map.Layers.Add(layWA_Bounds);
            map.Layers.Add(layPublic);
           // map.Layers.Add(layHydromjr);
	
            SharpMap.Geometries.BoundingBox mapExtents = map.GetExtents();
            map.Zoom = mapExtents.Width;
            map.MaximumZoom = mapExtents.Width;
            map.MinimumZoom = 2000;
            map.Center = mapExtents.GetCentroid();
            map.BackColor = Color.White;
            map.ZoomToExtents();

			HttpContext.Current.Trace.Write("Map initialized");
			return map;
    }
}
