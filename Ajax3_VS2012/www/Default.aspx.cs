using System;
using System.Drawing;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using GeoAPI.Geometries;
using SharpMap.Layers;
using SharpMap.Utilities.Wfs;


public partial class Ajax : System.Web.UI.Page//, ICallbackEventHandler
{

    protected void Page_Load(object sender, EventArgs e)
    {
        //Set up the map. We use the method in the App_Code folder for initializing the map
        ajaxMap.Map = MapHelper.InitializeMap(new System.Drawing.Size(10, 10), null, null);
        ajaxMap.FadeSpeed = 5;
        ajaxMap.ZoomSpeed = 5;
        Envelope mapExtents = (Envelope)ajaxMap.Map.GetExtents();
        ajaxMap.Map.Zoom = mapExtents.Width;
        ajaxMap.Map.Center = mapExtents.Centre;
        ajaxMap.ResponseFormat = "maphandler.aspx?MAP=SimpleWorld&Width=[WIDTH]&Height=[HEIGHT]&Zoom=[ZOOM]&X=[X]&Y=[Y]&Layers=[LAYERS]";
        // ajaxMap.OnClickEvent = map_Click();
        createCheckboxes(ajaxMap.Map.Layers);
    }
    /* protected void map_Click(object sender, EventArgs e)
     {
         //Set center of the map to where the client clicked
         Point ClickPnt = ajaxMap.Map.ImageToWorld(new System.Drawing.Point(e.X, e.Y)); ;

             SharpMap.Data.FeatureDataSet ds = new SharpMap.Data.FeatureDataSet();
             //Execute click-query
             (ajaxMap.Map.Layers[0] as SharpMap.Layers.VectorLayer).DataSource.ExecuteIntersectionQuery(ClickPnt, ds);
             if (ds.Tables.Count > 0) //We have a result
             {
                 clickresults.DataSource = ds.Tables[0];
                 clickresults.DataBind();
                 //Add clicked features to a selection layer
                 SharpMap.Layers.VectorLayer laySelected = new SharpMap.Layers.VectorLayer("Selection");
                 laySelected.DataSource = new GeometryProvider(ds.Tables[0]);
                 laySelected.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Yellow);
                 ajaxMap.Map.Layers.Add(laySelected);
             }
         GenerateMap();
     }*/

    private void createCheckboxes(LayerCollection LayerList)
    {
        Int32 i = 0;
        foreach (SharpMap.Layers.Layer myLayer in LayerList)
        {
            CheckBox chk = new CheckBox();
            chk.ID = "chk" + i.ToString();
            chk.Checked = true;

            if (myLayer.GetType() == typeof(SharpMap.Layers.LabelLayer))
                setLabel(chk, (myLayer as SharpMap.Layers.LabelLayer));

            if (myLayer.GetType() == typeof(SharpMap.Layers.LayerGroup))
            {
                chk.Attributes.Add("OnClick", "SharpMap_HiddenLayers(ajaxMapObj);togLayerGroup(this, '" + chk.ID.Substring(3) + "');test(ajaxMapObj);");
                pnlLayers.Controls.Add(chk);
                HtmlImage exCol = new HtmlImage();
                exCol.ID = "img" + i.ToString();
                exCol.Src = "images/minus.gif";
                exCol.Style.Add("cursor", "hand");
                exCol.Attributes.Add("onclick", "expandIt('" + chk.ID.Substring(3) + "');");
                pnlLayers.Controls.Add(exCol);
                Label myLabel = new Label();
                myLabel.Text = " " + myLayer.LayerName;
                pnlLayers.Controls.Add(myLabel);
            }
            else
            {
                chk.Attributes.Add("OnClick", "SharpMap_HiddenLayers(ajaxMapObj);test(ajaxMapObj);");
                chk.Text = myLayer.LayerName;
                pnlLayers.Controls.Add(chk);

                DropDownList listFill = new DropDownList();
                DropDownList listLine = new DropDownList();
                listFill.ID = "ColorPicker1" + myLayer.LayerName;
                listLine.ID = "ColorPicker2" + myLayer.LayerName;

                Type colorType = typeof(System.Drawing.Color);
                // We take only static property to avoid properties like Name, IsSystemColor ...
                PropertyInfo[] propInfos = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);
                foreach (PropertyInfo propInfo in propInfos)
                {
                    listFill.Items.Add(propInfo.Name);
                    listLine.Items.Add(propInfo.Name);
                }

                //pnlLayers.Controls.Add(new LiteralControl("<br>"));
                //pnlLayers.Controls.Add(new LiteralControl("Fill Color:"));

                listFill.SelectedValue = ((SolidBrush)((SharpMap.Styles.VectorStyle)myLayer.Style).Fill).Color.Name;
                listFill.Attributes.Add("onChange", "javascript:test(ajaxMapObj)");
                listFill.Attributes.Add("tag", myLayer.LayerName);
                pnlLayers.Controls.Add(listFill);

                //pnlLayers.Controls.Add(new LiteralControl("<br>"));
                //pnlLayers.Controls.Add(new LiteralControl("Line Color:"));

                listLine.SelectedValue = ((SharpMap.Styles.VectorStyle)myLayer.Style).Line.Color.Name;
                listLine.Attributes.Add("onChange", "javascript:test(ajaxMapObj)");
                listLine.Attributes.Add("tag", myLayer.LayerName);
                pnlLayers.Controls.Add(listLine);
            }

            pnlLayers.Controls.Add(new LiteralControl("<br>"));

            if (myLayer.GetType() == typeof(SharpMap.Layers.LayerGroup))
                recursiveCheck(pnlLayers, myLayer, i, chk.ID.ToString());
            i += 1;
        }
    }
    private void recursiveCheck(Control parentControl, SharpMap.Layers.ILayer myLayer, Int32 i, string chkID)
    {
        Int32 i2 = 0;
        HtmlGenericControl myDiv = new HtmlGenericControl("div");
        myDiv.Style.Add("margin-left", "1.5em");
        myDiv.ID = "myDiv" + chkID.Substring(3);
        parentControl.Controls.Add(myDiv);
        foreach (SharpMap.Layers.ILayer subLayer in (myLayer as SharpMap.Layers.LayerGroup).Layers)
        {
            CheckBox chk = new CheckBox();
            chk.ID = chkID + "_" + i2.ToString();
            chk.Checked = true;

            if (subLayer.GetType() == typeof(SharpMap.Layers.LabelLayer))
                setLabel(chk, (subLayer as SharpMap.Layers.LabelLayer));

            if (subLayer.GetType() == typeof(SharpMap.Layers.LayerGroup))
            {
                chk.Attributes.Add("OnClick", "SharpMap_HiddenLayers(ajaxMapObj);togLayerGroup(this, '" + chk.ID.Substring(3) + "');test(ajaxMapObj);");
                myDiv.Controls.Add(chk);
                HtmlImage exCol = new HtmlImage();
                exCol.ID = "img" + i.ToString();
                exCol.Src = "images/minus.gif";
                exCol.Style.Add("cursor", "hand");
                exCol.Attributes.Add("onclick", "expandIt('" + chk.ID.Substring(3) + "');");
                myDiv.Controls.Add(exCol);
                Label myLabel = new Label();
                myLabel.Text = " " + subLayer.LayerName;
                myDiv.Controls.Add(myLabel);
                recursiveCheck((myDiv as Control), subLayer, i2, chk.ID.ToString());
            }
            else
            {
                chk.Attributes.Add("OnClick", "SharpMap_HiddenLayers(ajaxMapObj);test(ajaxMapObj);");
                chk.Text = subLayer.LayerName;
                myDiv.Controls.Add(chk);
               // if (subLayer.GetType() == typeof(SharpMap.Layers.VectorLayer))
               //     myDiv.Controls.Add(LegendDiv(subLayer));
                myDiv.Controls.Add(new LiteralControl("<br>"));
            }
            i2 += 1;
        }
    }
    private static string ColorToHex(Color color)
    {
        string toReturn = string.Format("0x{0:X8}", color.ToArgb()); toReturn = "#" + toReturn.Substring(toReturn.Length - 6, 6);
        return toReturn;
    }
    private static Double ColorAlpha(Color color)
    {
        Double myAlpha = Double.Parse(color.A.ToString());
        return Math.Round((myAlpha / 255) * 100);
    }
    private void setLabel(CheckBox chk, SharpMap.Layers.LabelLayer myLabel)
    {
        chk.Style.Add("font-family", myLabel.Style.Font.FontFamily.Name);
        chk.Style.Add("color", ColorToHex(myLabel.Style.ForeColor));
        chk.Style.Add("background-color", ColorToHex((myLabel.Style.BackColor as SolidBrush).Color));
        chk.Style.Add("font-weight", myLabel.Style.Font.Style.ToString());
        chk.Style.Add("font-size", myLabel.Style.Font.Size.ToString() + "px");
    }
    private HtmlGenericControl LegendDiv(SharpMap.Layers.ILayer myLayer)
    {
        HtmlGenericControl legendDiv = new HtmlGenericControl("div");
        legendDiv.Style.Add("width", "2em");
        legendDiv.Style.Add("overflow", "hidden");
        legendDiv.Style.Add("position", "absolute");
        legendDiv.Style.Add("margin-left", "0.5em");
        legendDiv.Style.Add("margin-bottom", "0px");
        if (myLayer.GetType() == typeof(SharpMap.Layers.VectorLayer))
        {
            SharpMap.Layers.VectorLayer myVectorLayer = (myLayer as SharpMap.Layers.VectorLayer);

            SharpMap.Styles.VectorStyle myStyle = new SharpMap.Styles.VectorStyle();
            if (myVectorLayer.Theme != null)
            {
                if (myVectorLayer.Theme.GetType() == typeof(SharpMap.Rendering.Thematics.CustomTheme))
                {
                    SharpMap.Rendering.Thematics.CustomTheme myTheme = (myVectorLayer.Theme as SharpMap.Rendering.Thematics.CustomTheme);
                    myStyle = (myTheme.DefaultStyle as SharpMap.Styles.VectorStyle);
                }
                else if (myVectorLayer.Theme.GetType() == typeof(SharpMap.Rendering.Thematics.GradientTheme))
                {
                    SharpMap.Rendering.Thematics.GradientTheme myTheme = (myVectorLayer.Theme as SharpMap.Rendering.Thematics.GradientTheme);
                    myStyle = (myTheme.MinStyle as SharpMap.Styles.VectorStyle);
                }
            }
            else
                myStyle = myVectorLayer.Style;

            if (myStyle.Outline.Color.Name.ToString() != "Black")
            {
                string lineStyle = (myStyle.Outline.DashStyle.ToString() == "Dash") ? "dotted" : "solid";
                legendDiv.Style.Add("border", lineStyle + " " + myStyle.Outline.Width.ToString() + "px " + ColorToHex(myStyle.Outline.Color));
                legendDiv.Style.Add("height", "1em");
            }
            else
            {
                string lineStyle = (myStyle.Line.DashStyle.ToString() == "Dash") ? "dotted" : "solid";
                legendDiv.Style.Add("border-bottom", lineStyle + " " + myStyle.Line.Width.ToString() + "px " + ColorToHex(myStyle.Line.Color));
                legendDiv.Style.Add("height", "0.8em");
            }

            System.Drawing.SolidBrush fillBrush = (myVectorLayer.Style.Fill as System.Drawing.SolidBrush);
            if (fillBrush.Color.Name.ToString() != "Black")
            {
                HtmlGenericControl fillDiv = new HtmlGenericControl("div");
                fillDiv.Style.Add("border-left", "solid 2em " + ColorToHex(fillBrush.Color));
                fillDiv.Style.Add("height", "1.2em");
                fillDiv.Style.Add("overflow", "hidden");
                fillDiv.Style.Add("opacity ", ColorAlpha(fillBrush.Color) + "%");
                fillDiv.Style.Add("filter", "ALPHA(opacity=" + ColorAlpha(fillBrush.Color) + ")");
                legendDiv.Controls.Add(fillDiv);
            }
        }
        return legendDiv;

        
    }
}

