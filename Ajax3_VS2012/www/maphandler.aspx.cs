using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// The maphandler class takes a set of GET or POST parameters and returns a map as PNG (this reminds in many ways of the way a WMS server work).
/// Required parameters are: WIDTH, HEIGHT, ZOOM, X, Y, MAP
/// </summary>
public partial class maphandler : System.Web.UI.Page
{
	internal static System.Globalization.NumberFormatInfo numberFormat_EnUS = new System.Globalization.CultureInfo("en-US", false).NumberFormat;

    protected void Page_Load(object sender, EventArgs e)
	{
		int Width = 0;
		int Height = 0;
		double centerX = 0;
		double centerY = 0;
		double Zoom = 0;
        string[] Layer;

        //Parse request parameters
		if(!int.TryParse(Request.Params["WIDTH"],out Width))
			throw(new ArgumentException("Invalid parameter"));
		if(!int.TryParse(Request.Params["HEIGHT"], out Height))
			throw (new ArgumentException("Invalid parameter"));
		if(!double.TryParse(Request.Params["ZOOM"], System.Globalization.NumberStyles.Float, numberFormat_EnUS, out Zoom))
			throw (new ArgumentException("Invalid parameter"));
		if(!double.TryParse(Request.Params["X"], System.Globalization.NumberStyles.Float, numberFormat_EnUS, out centerX))
			throw (new ArgumentException("Invalid parameter"));
		if(!double.TryParse(Request.Params["Y"], System.Globalization.NumberStyles.Float, numberFormat_EnUS, out centerY))
			throw (new ArgumentException("Invalid parameter"));
		if(Request.Params["MAP"]==null)
			throw (new ArgumentException("Invalid parameter"));
        if (!string.IsNullOrEmpty(Request.Params["Layers"]))
            Layer = Request.Params["Layers"].Split(new char[]{','});
        else throw (new ArgumentException("Invalid parameter"));

        string colors = Request.Params["Colors"];
        string colorsLine = Request.Params["ColorsLine"];
        
		//Params OK
        SharpMap.Map map = InitializeMap(Request.Params["MAP"], new System.Drawing.Size(Width, Height), colors, colorsLine);
		if(map==null)
			throw (new ArgumentException("Invalid map"));

        //toggle layers 
        if(Layer[0]!="none")
            for (int i=0;i<Layer.Length;i++)
                toggleLayer(Layer[i], map).Enabled = false;


		//Set visible map extents
		map.Center = new GeoAPI.Geometries.Coordinate(centerX, centerY);
		map.Zoom = Zoom;

        

		//Generate map
		System.Drawing.Bitmap img = (System.Drawing.Bitmap) map.GetMap();

		//Stream the image to the client
		Response.ContentType = "image/png";
		System.IO.MemoryStream MS = new System.IO.MemoryStream();
		img.Save(MS, System.Drawing.Imaging.ImageFormat.Png);

		// tidy up  
		img.Dispose();
		byte[] buffer = MS.ToArray();
		Response.OutputStream.Write(buffer, 0, buffer.Length);
	}

    private SharpMap.Layers.ILayer toggleLayer(string id, SharpMap.Map map)
    {
        string[] layerId = id.Split(new char[] { '_' });
        if (layerId.Length<=1)
            return map.Layers[Int32.Parse(layerId[0])];

        SharpMap.Layers.LayerGroup myGroup = (map.Layers[Int32.Parse(layerId[0])] as SharpMap.Layers.LayerGroup);

        for(int i=1;i<layerId.Length-1;i++)
            {
                myGroup = (myGroup.Layers[Int32.Parse(layerId[i])] as SharpMap.Layers.LayerGroup);
            }
            return myGroup.Layers[Int32.Parse(layerId[layerId.Length-1])];
    }

    private SharpMap.Map InitializeMap(string MapID, System.Drawing.Size size, string colors, string colorsLine)
	{
		//Set up the map. We use the method in the App_Code folder for initializing the map
		switch (MapID)
		{
			//Our simple world map was requested 
			case "SimpleWorld":
                return MapHelper.InitializeMap(size, colors, colorsLine);
			default:
				throw new ArgumentException("Invalid map '" + MapID + "' requested");
		}
	}
}
