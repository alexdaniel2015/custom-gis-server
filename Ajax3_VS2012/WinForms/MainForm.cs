using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BruTile;
using BruTile.Web;
using SharpMap;
using SharpMap.Forms;
using SharpMap.Layers;

namespace WinForms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Map map = InitializeGoogle(GoogleMapType.GoogleSatellite);

            //var tileLayer = new SharpMap.Layers.TileAsyncLayer(new BruTile.Web.OsmTileSource(), "TileLayer - OSM");
            //map.BackgroundLayer.Add(tileLayer);
            //map.ZoomToBox(tileLayer.Envelope);

            mapBox1.Map = map;
            mapBox1.Map.ZoomToExtents();
            mapBox1.ActiveTool = MapBox.Tools.Pan;

        }

        public static SharpMap.Map InitializeGoogle(GoogleMapType mt)
        {
            SharpMap.Map map = new SharpMap.Map();

            GoogleRequest req;
            ITileSource tileSource;
            TileLayer tileLayer;
            if (mt == (GoogleMapType.GoogleSatellite | GoogleMapType.GoogleLabels))
            {
                req = new GoogleRequest(GoogleMapType.GoogleSatellite);
                tileSource = new GoogleTileSource(req);
                tileLayer = new TileLayer(tileSource, "TileLayer - " + GoogleMapType.GoogleSatellite);
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

            tileLayer = new TileLayer(tileSource, "TileLayer - " + mt);
            map.Layers.Add(tileLayer);
            map.ZoomToBox(tileLayer.Envelope);
            return map;
        }



    }
}
