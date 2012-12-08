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
using BruTile;
using BruTile.Web;
using GeoAPI.Geometries;
using SharpMap;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using SharpMap.Styles;
using System.Collections.Generic;
using SharpMap.Rendering.Thematics;

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


        var lvivLayer = new VectorLayer("Lviv");
        lvivLayer.DataSource = new ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\MapLviv\block.shp"), true);
        var pen = new Pen(Color.FromArgb(255, 0, 0, 1));
        lvivLayer.Style.Outline = pen;
        lvivLayer.Style.Fill = new SolidBrush(Color.FromArgb(255, 234, 234, 234));
        lvivLayer.Style.EnableOutline = true;

        var lvivFuelCompaniesLayer = new VectorLayer("Lviv fuel companies");
        lvivFuelCompaniesLayer.DataSource = new ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\MapStations\fuel_stations_lviv.shp"), true);

        lvivFuelCompaniesLayer.Style.Outline = pen;
        lvivFuelCompaniesLayer.Style.Fill = new SolidBrush(Color.FromArgb(222, 90, 97, 234));
        lvivFuelCompaniesLayer.Style.EnableOutline = true;

        #region NationalPark layers
        /*
        //Set up the boundaries layer
        SharpMap.Layers.VectorLayer layWA_Bounds = new SharpMap.Layers.VectorLayer("Washington State");
        layWA_Bounds.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\NationalPark\wa_bndry.shp"), true);
        //layWA_Bounds.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\Lviv\regions.shp"), true);
         
        Pen WAPen = new Pen(Color.FromArgb(255, 0, 0, 1));
        layWA_Bounds.Style.Outline = WAPen;
        layWA_Bounds.Style.Fill = new SolidBrush(Color.FromArgb(255, 234, 234, 234));
        layWA_Bounds.Style.EnableOutline = true;


        //Set up Streams layer
        SharpMap.Layers.VectorLayer layHydromjr = new SharpMap.Layers.VectorLayer("Streams");
        layHydromjr.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\NationalPark\wa_hydro.shp"), true);
        layHydromjr.Style.Line = new Pen(Color.FromArgb(255, 0, 100, 255));
        layHydromjr.MaxVisible = 70000;

        VectorStyle newStyle = new VectorStyle();
        newStyle.Fill = new SolidBrush(Color.FromArgb(0, 0, 0));
        Dictionary<string, IStyle> styles = new Dictionary<string, IStyle>();
        styles.Add("newStyle", newStyle);
        
        //layHydromjr.Theme = theme;

        //Set up National Park  area layer
        SharpMap.Layers.VectorLayer layWA_ParkAreas = new SharpMap.Layers.VectorLayer("Park Area");
        layWA_ParkAreas.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\NationalPark\natpark.shp"), true);
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
        layWA_Forests.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\NationalPark\natfor.shp"), true);
        layWA_Forests.Style.Outline = new Pen(Color.FromArgb(200, 165, 165, 73));
        layWA_Forests.Style.Fill = new SolidBrush(Color.FromArgb(200, 236, 241, 185));
        layWA_Forests.Style.EnableOutline = true;

        //Set up National Rec layer
        SharpMap.Layers.VectorLayer layWA_RecArea = new SharpMap.Layers.VectorLayer("National Rec Areas");
        layWA_RecArea.DataSource = new SharpMap.Data.Providers.ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\NationalPark\natrec.shp"), true);
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
        */
        #endregion

        map.Layers.Add(lvivLayer);
        map.Layers.Add(lvivFuelCompaniesLayer);

        Envelope mapExtents = map.GetExtents();
        map.Zoom = mapExtents.Width;
        map.MaximumZoom = mapExtents.Width;
        map.MinimumZoom = 2000;
        map.Center = mapExtents.Centre;
        map.BackColor = Color.White;
        map.ZoomToExtents();

        HttpContext.Current.Trace.Write("Map initialized");
        return map;
    }

    public static SharpMap.Map InitializeGoogleMap(GoogleMapType mt)
    {
        SharpMap.Map map = new SharpMap.Map();

        GoogleRequest req;
        ITileSource tileSource;
        TileAsyncLayer tileLayer;

        if (mt == (GoogleMapType.GoogleSatellite | GoogleMapType.GoogleLabels))
        {
            req = new GoogleRequest(GoogleMapType.GoogleSatellite);
            tileSource = new GoogleTileSource(req);
            tileLayer = new TileAsyncLayer(tileSource, "TileLayer - " + GoogleMapType.GoogleSatellite);
            map.Layers.Add(tileLayer);
            req = new GoogleRequest(GoogleMapType.GoogleLabels);
            tileSource = new GoogleTileSource(req);
            mt = GoogleMapType.GoogleLabels;
        }
        else
        {
            req = new GoogleRequest(mt);
            tileSource = new GoogleTileSource(req);
        }

        tileLayer = new TileAsyncLayer(tileSource, "TileLayer - " + mt);
        map.BackgroundLayer.Add(tileLayer);
        map.ZoomToBox(tileLayer.Envelope);
        return map;
    }



    //public void CreateMapWithExcelDataSource()
    //{
    //    var map = new SharpMap.Map(new System.Drawing.Size(700, 700));

    //    //Get background
    //    var osmLayer = new SharpMap.Layers.TileLayer(new BruTile.Web.OsmTileSource(), "OSM");
    //    map.Layers.Add(osmLayer);

    //    //Get data from excel
    //    var xlsPath = string.Format(XlsConnectionString, System.IO.Directory.GetCurrentDirectory(), "Cities.xls");
    //    var ds = new System.Data.DataSet("XLS");
    //    using (var cn = new System.Data.OleDb.OleDbConnection(xlsPath))
    //    {
    //        cn.Open();
    //        using (var da = new System.Data.OleDb.OleDbDataAdapter(new System.Data.OleDb.OleDbCommand("SELECT * FROM [Cities$]", cn)))
    //            da.Fill(ds);
    //    }

    //    //Set up provider
    //    var xlsProvider = new SharpMap.Data.Providers.DataTablePoint(ds.Tables[0], "OID", "X", "Y");
    //    var xlsLayer = new SharpMap.Layers.VectorLayer("XLS", xlsProvider);
    //    xlsLayer.Style.Symbol = new System.Drawing.Bitmap("DefaultSymbol.png");

    //    //The SRS for this datasource is EPSG:4326, therefore we need to transfrom it to OSM projection
    //    var ctf = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
    //    var cf = new ProjNet.CoordinateSystems.CoordinateSystemFactory();
    //    var epsg4326 = cf.CreateFromWkt("GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]");
    //    var epsg3857 = cf.CreateFromWkt("PROJCS[\"Popular Visualisation CRS / Mercator\", GEOGCS[\"Popular Visualisation CRS\", DATUM[\"Popular Visualisation Datum\", SPHEROID[\"Popular Visualisation Sphere\", 6378137, 0, AUTHORITY[\"EPSG\",\"7059\"]], TOWGS84[0, 0, 0, 0, 0, 0, 0], AUTHORITY[\"EPSG\",\"6055\"]],PRIMEM[\"Greenwich\", 0, AUTHORITY[\"EPSG\", \"8901\"]], UNIT[\"degree\", 0.0174532925199433, AUTHORITY[\"EPSG\", \"9102\"]], AXIS[\"E\", EAST], AXIS[\"N\", NORTH], AUTHORITY[\"EPSG\",\"4055\"]], PROJECTION[\"Mercator\"], PARAMETER[\"False_Easting\", 0], PARAMETER[\"False_Northing\", 0], PARAMETER[\"Central_Meridian\", 0], PARAMETER[\"Latitude_of_origin\", 0], UNIT[\"metre\", 1, AUTHORITY[\"EPSG\", \"9001\"]], AXIS[\"East\", EAST], AXIS[\"North\", NORTH], AUTHORITY[\"EPSG\",\"3857\"]]");
    //    xlsLayer.CoordinateTransformation = ctf.CreateFromCoordinateSystems(epsg4326, epsg3857);

    //    //Add layer to map
    //    map.Layers.Add(xlsLayer);

    //    //Zoom to map
    //    map.ZoomToExtents();
    //    var img = map.GetMap();
    //    img.Save("osmexcel.bmp");

    //}



















}
