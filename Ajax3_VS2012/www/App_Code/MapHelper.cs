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
    public static SharpMap.Map InitializeMap(Size size, string colors, string colorsLine)
    {
        HttpContext.Current.Trace.Write("Initializing map...");

        //Initialize a new map of size 'imagesize'
        var map = new SharpMap.Map(size);

        var blackPen = new Pen(Color.FromArgb(255, 0, 0, 0), 1);
        var blackPen2 = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
        var greenPen = new Pen(Color.FromArgb(255, 0, 255, 0), 1);
        var greenPenTransp = new Pen(Color.FromArgb(255, 0, 255, 0), 1);
        greenPenTransp.Color = Color.FromArgb(100, greenPenTransp.Color.R, greenPenTransp.Color.G, greenPenTransp.Color.B);
        var redPen = new Pen(Color.FromArgb(255, 255, 0, 0), 1);
        var bluePen = new Pen(Color.FromArgb(255, 0, 0, 255), 1);
        var violetPen = new Pen(Color.FromArgb(255, 255, 0, 255), 1);
        var cyanPen = new Pen(Color.FromArgb(255, 0, 255, 255), 1);
        var yellowPen = new Pen(Color.FromArgb(255, 255, 255, 0), 1);
        var darkGrayPen = new Pen(Color.DarkGray, 1);
        var darkGrayPenTransp = new Pen(Color.DarkGray, 1);
        darkGrayPenTransp.Color = Color.FromArgb(100, darkGrayPenTransp.Color.R, darkGrayPenTransp.Color.G, darkGrayPenTransp.Color.B);
        var lightBluePen = new Pen(Color.LightBlue, 1);
        var lightGreenPen = new Pen(Color.LightGreen, 1);
        var lightGrayPen = new Pen(Color.FromArgb(255, 234, 234, 234));
        var darkGreenPen = new Pen(Color.DarkGreen);
        var azurePen = new Pen(Color.DarkKhaki);
        var lightGoldenrodYellowPen = new Pen(Color.LightGoldenrodYellow);

        Dictionary<string, Color> colorFill = new Dictionary<string, Color>();

        
        if (colors != null)
        {
            colors = colors.Replace(",", "");
            string[] parts = colors.Split('|');

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] != "")
                {
                    string[] pair = parts[i].Split('=');

                    try
                    {
                        colorFill.Add(pair[0], Color.FromName(pair[1]));
                    }
                    catch { }
                }
            }
        }
        else
        {
            colorFill["Lviv"] = Color.FromArgb(255, 234, 234, 234);
            colorFill["Suburbs2"] = lightGrayPen.Color;
            colorFill["Parks"] = darkGreenPen.Color;
            colorFill["Suburbs1"] = lightGrayPen.Color;
            colorFill["Homes"] = azurePen.Color;
            colorFill["Proms"] = darkGrayPen.Color;
            colorFill["FuelStations"] = Color.FromArgb(255, 234, 234, 234);
            
        }

        Dictionary<string, Color> colorLineDict = new Dictionary<string, Color>();

        
        if (colors != null)
        {
            colorsLine = colorsLine.Replace(",", "");
            string[] parts = colorsLine.Split('|');

            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] != "")
                {
                    string[] pair = parts[i].Split('=');

                    try
                    {
                        colorLineDict.Add(pair[0], Color.FromName(pair[1]));
                    }
                    catch { }
                }
            }
        }
        else
        {
            colorLineDict["Lviv"] = blackPen.Color;
            colorLineDict["Suburbs2"] = blackPen.Color;
            colorLineDict["Parks"] = blackPen.Color;
            colorLineDict["Suburbs1"] = blackPen.Color;
            colorLineDict["Homes"] = blackPen.Color;
            colorLineDict["Proms"] = blackPen.Color;
            colorLineDict["FuelStations"] = blackPen.Color;
        }

        bool town = true;
        if (town)
        {
            var lvivBlockLayer = new VectorLayer("Lviv");
            lvivBlockLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\maps_lviv_new\block.shp"), true);
            lvivBlockLayer.Style.Outline = new Pen(colorLineDict["Lviv"]);
            lvivBlockLayer.Style.Fill = new SolidBrush( colorFill["Lviv"] );
            lvivBlockLayer.Style.EnableOutline = true;
            map.Layers.Add(lvivBlockLayer);

            var suburbsLayer2 = new VectorLayer("Suburbs2");
            suburbsLayer2.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(@"~\App_Data\maps_lviv_new\LLSITY12.shp"), true);
            suburbsLayer2.Style.Outline = new Pen(colorLineDict["Suburbs2"]);
            suburbsLayer2.Style.Fill = new SolidBrush(colorFill["Suburbs2"]);
            suburbsLayer2.Style.EnableOutline = true;
            map.Layers.Add(suburbsLayer2);

            var parksLayer = new VectorLayer("Parks");
            parksLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(@"~\App_Data\maps_lviv_new\LLVGREN1.shp"),
                true);
            parksLayer.Style.Outline = new Pen(colorLineDict["Parks"]);
            parksLayer.Style.Fill = new SolidBrush(colorFill["Parks"]);
            parksLayer.Style.EnableOutline = true;
            map.Layers.Add(parksLayer);

            var suburbsLayer1 = new VectorLayer("Suburbs1");
            suburbsLayer1.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(@"~\App_Data\maps_lviv_new\LLVELL12.shp"),
                true);
            suburbsLayer1.Style.Outline = new Pen(colorLineDict["Suburbs1"]);
            suburbsLayer1.Style.Fill = new SolidBrush(colorFill["Suburbs1"]);
            suburbsLayer1.Style.EnableOutline = true;
            map.Layers.Add(suburbsLayer1);

            //homes
            var homesLayer = new VectorLayer("Homes");
            homesLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(@"~\App_Data\maps_lviv_new\LLVHOM12.shp"),
                true);
            homesLayer.Style.Outline = new Pen(colorLineDict["Homes"]);
            homesLayer.Style.Fill = new SolidBrush(colorFill["Homes"]);
            homesLayer.Style.EnableOutline = true;
            map.Layers.Add(homesLayer);

            //proms
            var promLayer = new VectorLayer("Proms");
            promLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(@"~\App_Data\maps_lviv_new\LLVPROM.shp"), true);
            promLayer.Style.Outline = new Pen(colorLineDict["Proms"]);
            promLayer.Style.Fill = new SolidBrush(colorFill["Proms"]);
            promLayer.Style.EnableOutline = true;
            map.Layers.Add(promLayer);

            VectorLayer fuelStationLayer = new VectorLayer("FuelStations");
            fuelStationLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\FuelStationsLayers\LvivCityCorr\fuel_stations_lviv.shp"),
                true);
            fuelStationLayer.Style.Outline = new Pen(colorLineDict["FuelStations"]);
            fuelStationLayer.Style.Fill = new SolidBrush(colorFill["FuelStations"]);
            fuelStationLayer.Style.EnableOutline = true;
            //fuelStationLayer.Style.PointColor = Brushes.AliceBlue;
            SharpMap.Rendering.Thematics.CustomTheme myTheme = new SharpMap.Rendering.Thematics.CustomTheme(GetFullStationStyle);
            fuelStationLayer.Theme = myTheme;
            map.Layers.Add(fuelStationLayer);

            //var tmp6 = new VectorLayer("Lviv"){DataSource = new ShapeFile(HttpContext.Current.Server.MapPath(@"~\App_Data\maps_lviv_new\LROAD12.shp"), true)};
            //tmp6.Style.Outline = waPen6;
            //tmp6.Style.Fill = new SolidBrush(Color.FromArgb(255, 234, 234, 234));
            //tmp6.Style.EnableOutline = true;
            //map.Layers.Add(tmp6);
        }
        else
        {
            var lvivRegioonLayer = new VectorLayer("Region");
            lvivRegioonLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\Lviv_Region_Romik\lv_line.shp"), true);
            lvivRegioonLayer.Style.Outline = blackPen2;
            lvivRegioonLayer.Style.Fill = new SolidBrush(Color.FromArgb(255, 234, 234, 234));
            lvivRegioonLayer.Style.EnableOutline = true;
            map.Layers.Add(lvivRegioonLayer);

            var territoryLayer = new VectorLayer("Regions");
            territoryLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\Lviv_Region_Romik\terrytor.shp"), true);
            territoryLayer.Style.Outline = darkGrayPenTransp;
            territoryLayer.Style.Fill = new SolidBrush(lightGoldenrodYellowPen.Color);
            territoryLayer.Style.EnableOutline = true;
            map.Layers.Add(territoryLayer);

            var lakesLayer = new VectorLayer("Lakes");
            lakesLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\Lviv_Region_Romik\vodoimy.shp"), true);
            lakesLayer.Style.Outline = blackPen;
            lakesLayer.Style.Fill = new SolidBrush(bluePen.Color);
            lakesLayer.Style.EnableOutline = true;
            map.Layers.Add(lakesLayer);

            var punkLayer = new VectorLayer("Punks");
            punkLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\Lviv_Region_Romik\nas_punk.shp"), true);
            punkLayer.Style.Outline = blackPen;
            punkLayer.Style.Fill = new SolidBrush(darkGrayPen.Color);
            punkLayer.Style.EnableOutline = true;
            map.Layers.Add(punkLayer);

            var railsLayer = new VectorLayer("Rails");
            railsLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\Lviv_Region_Romik\zal_kol.shp"), true);
            //map.Layers.Add(railsLayer);

            var fuelStationLayer = new VectorLayer("FuelStations");
            fuelStationLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\FuelStationsLayers\LvivObl\fuel_stations.shp"),
                true);
            fuelStationLayer.Style.PointColor = Brushes.AliceBlue;
            map.Layers.Add(fuelStationLayer);

            var roadsLayer = new VectorLayer("Roads");
            roadsLayer.Style.Outline = blackPen;
            roadsLayer.DataSource = new ShapeFile(
                HttpContext.Current.Server.MapPath(
                    @"~\App_Data\Lviv_Region_Romik\meregi.shp"), true);
            map.Layers.Add(roadsLayer);


        }

        GeoAPI.Geometries.Envelope mapExtents = map.GetExtents();
        //SharpMap.Geometries.BoundingBox mapExtents = map.GetExtents();
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


    private static SharpMap.Styles.VectorStyle GetFullStationStyle(SharpMap.Data.FeatureDataRow row)
    {
        SharpMap.Styles.VectorStyle style = new SharpMap.Styles.VectorStyle();
        string s1 = row["Oid"].ToString();
        string s2 = row["id"].ToString();

        if (Int32.Parse(s1) < 6)
        {
            style.PointColor = new SolidBrush(Color.DarkOrange);
            style.PointSize = 20f;
        }
        else
        {
            style.PointColor = new SolidBrush(Color.DeepSkyBlue);
            style.PointSize = 10f;
        }
      
        //switch (row["NAME"].ToString().ToLower())
        //{
        //    case "denmark": //If country name is Danmark, fill it with green
        //        style.Fill = Brushes.Green;
        //        return style;
        //    case "united states": //If country name is USA, fill it with Blue and add a red outline
        //        style.Fill = Brushes.Blue;
        //        style.Outline = Pens.Red;
        //        return style;
        //    case "china": //If country name is China, fill it with red
        //        style.Fill = Brushes.Red;
        //        return style;
        //    default:
        //        break;
        //}
        ////If country name starts with S make it yellow
        //if (row["NAME"].ToString().StartsWith("S"))
        //{
        //    style.Fill = Brushes.Yellow;
        //    return style;
        //}
        //// If geometry is a (multi)polygon and the area of the polygon is less than 30, make it cyan
        //else if (row.Geometry is GeoAPI.Geometries.IPolygonal && row.Geometry.Area < 30)
        //{
        //    style.Fill = Brushes.Cyan;
        //    return style;
        //}
        //else //None of the above -> Use the default style
            return style ;
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
