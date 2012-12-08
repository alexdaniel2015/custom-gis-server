// Copyright 2005, 2006 - Morten Nielsen (www.iter.dk)
//
// This file is part of SharpMap.
// SharpMap is free software; you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
// 
// SharpMap is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.

// You should have received a copy of the GNU Lesser General Public License
// along with SharpMap; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA 

var metersToMiles = 0.0006213712

function SharpMap_Init(container, map1, map2, statusbar, statustext) {
    var obj = new Object();
    obj.currTool = "pan";
    obj.currMap = 1;
    obj.mapReady = 1;
    obj.zoomEnded = 1;
    obj.zoomAmount = 3.0;
    obj.hiddenLayers = "none";
    obj.clickEvent = null;
    obj.clickEventActive = false;
    obj.toggleClickEvent = function () { obj.clickEventActive = (!obj.clickEventActive); }
    obj.disableClickEvent = function () { obj.clickEventActive = false; }
    obj.enableClickEvent = function () { obj.clickEventActive = true; }
    obj.setClickEvent = function (fnc) { obj.clickEvent = fnc; }
    obj.container = WebForm_GetElementById(container);
    obj.map1 = WebForm_GetElementById(map1);
    obj.map2 = WebForm_GetElementById(map2);
    if (statusbar != '') { obj.statusbar = WebForm_GetElementById(statusbar); obj.statusText = statustext; }
    //Methods
    obj.VisibleMap = function () { if (obj.currMap == 1) return obj.map1; else return obj.map2; }
    obj.HiddenMap = function () { if (obj.currMap == 2) return obj.map1; else return obj.map2; }
    obj.GetCenter = function () { return SharpMap_GetCenter(obj); }
    //Events
    obj.container.onmousemove = function (event) { SharpMap_MapMouseOver(event, obj); }
    if (statusbar != '') obj.container.onmouseout = function (event) { obj.statusbar.innerHTML = ''; }
    obj.container.onmousewheel = function (event) { SharpMap_MouseWheel(event, obj); return false; }
    if (obj.container.addEventListener)
        obj.container.addEventListener('DOMMouseScroll', function (event) { SharpMap_MozillaMouseWheel(event, obj); }, false);
    obj.container.onresize = function (event) { SharpMap_ResizeTimeout(event, obj); }
    obj.container.onselectstart = function () { return false; }
    obj.container.ondrag = function (event) { return false; }
    obj.container.onmousedown = function (event) { SharpMap_MouseDown(event, obj); return false; }
    obj.container.onmouseup = function (event) { SharpMap_MouseUp(event, obj); return false; }
    document.onmouseup = function (event) { Outside_MouseUp(event, obj); return false; }

    return obj;
}

/* Lets the user select which tool to use */
function SharpMap_ToolSelect(obj, tool) {
    obj.currTool = tool;
}

/* Called when the mousewheel-scroll event occurs on the map  */
function SharpMap_MouseWheel(event, obj) {
    var e = event || window.event;
    if (e.type == 'mousewheel' && obj.mapReady && obj.zoomEnded == 1) {
        var zoomval = obj.zoomAmount;
        if (e.wheelDelta < 0) { zoomval = 1 / obj.zoomAmount; }
        SharpMap_BeginZoom(obj, e.clientX, e.clientY, zoomval);
    }
}
/* this intermediate wheel function is used for mousewheel compatibility in Mozilla browsers */
function SharpMap_MozillaMouseWheel(event, obj) {
    var e = new Object;
    e.type = 'mousewheel';
    e.wheelDelta = -event.detail;
    e.clientX = event.screenX;
    e.clientY = event.screenY;
    SharpMap_MouseWheel(e, obj);
}
var startDrag = null;
/* MouseDown - Occurs when potentially starting a drag event */
function SharpMap_MouseDown(event, obj) {
    var e = event || window.event;
    if (obj.zoomEnded == 1 && obj.mapReady == 1) {
        if (!SharpMap_IsDefined(startDrag)) {
            startDrag = SharpMap_GetRelativePosition(e.clientX, e.clientY, obj.container);
            switch (obj.currTool) {
                case "boxZoom":
                    obj.zoomLayer.style.visibility = "visible";
                    obj.zoomLayer.style.left = startDrag.x;
                    obj.zoomLayer.style.top = startDrag.y;
                    obj.zoomLayer.style.width = "0px";
                    obj.zoomLayer.style.height = "0px";
                    obj.container.style.cursor = "crosshair";
                    break;
                case "measure":
                    obj.container.style.cursor = "crosshair";
                    break;
            }
        }
    }
}


/* MouseUp - Occurs during a drag event or when doing a click */
function SharpMap_MouseUp(event, obj) {
    var e = event || window.event;
    if (obj.zoomEnded == 1 && obj.mapReady == 1 && SharpMap_IsDefined(startDrag) && SharpMap_IsDefined(endDrag)) {
        switch (obj.currTool) {
            case "pan":
                var dx = endDrag.x - startDrag.x;
                var dy = endDrag.y - startDrag.y;

                if (Math.abs(dx) > 4 || Math.abs(dy) > 4) //we are dragging
                    SharpMap_PanEnd(obj, dx, dy);
                else // we are not dragging so lets do the click event
                    SharpMap_BeginZoom(obj, e.clientX, e.clientY, obj.zoomAmount);

                break;
            case "boxZoom":
                obj.zoomLayer.style.visibility = "hidden";
                obj.container.style.cursor = "default";
                var zoomBox = SharpMap_GetBoundingBox(obj, startDrag, endDrag);
                SharpMap_BoxBeginZoom(obj, zoomBox.centerX, zoomBox.centerY, zoomBox.zoomAmount);
                break;
            case "measure":
                jg.clear();
                obj.measure.style.visibility = "hidden";
                obj.container.style.cursor = "default";
                break;
        }
    } else if (obj.clickEventActive && obj.clickEvent != null)
        obj.clickEvent(e, obj);
    else // we are not dragging so lets do the click event
        SharpMap_BeginZoom(obj, e.clientX, e.clientY, obj.zoomAmount);

    startDrag = null;
    endDrag = null;
    return false;
}

function SharpMap_PanEnd(obj, dx, dy) {
    var center = SharpMap_PixelToMap(obj.container.offsetWidth * 0.5 - dx, obj.container.offsetHeight * 0.5 - dy, obj);
    obj.minX = center.x - obj.zoom * 0.5;
    obj.maxY = center.y + obj.zoom / obj.container.offsetWidth * obj.container.offsetHeight * 0.5;
    obj.mapReady = 0;
    SharpMap_BeginRefreshMap(obj, 1);
}

function Outside_MouseUp(event, obj) {
    var e = event || window.event;
    if (e.button == 1 && obj.zoomEnded == 1 && obj.mapReady == 1 && SharpMap_IsDefined(startDrag) && SharpMap_IsDefined(endDrag)) {
        switch (obj.currTool) {
            case "pan":
                var dx = endDrag.x - startDrag.x;
                var dy = endDrag.y - startDrag.y;

                if (Math.abs(dx) > 4 || Math.abs(dy) > 4) //we are dragging
                    SharpMap_PanEnd(obj, dx, dy);
                else // we are not dragging so lets do the click event
                    SharpMap_BeginZoom(obj, e.clientX, e.clientY, obj.zoomAmount);

                break;
            case "boxZoom":
                obj.zoomLayer.style.visibility = "hidden";
                obj.container.style.cursor = "default";
                var zoomBox = SharpMap_GetBoundingBox(obj, startDrag, endDrag);
                SharpMap_BoxBeginZoom(obj, zoomBox.centerX, zoomBox.centerY, zoomBox.zoomAmount);
                break;
            case "measure":
                jg.clear();
                obj.measure.style.visibility = "hidden";
                obj.container.style.cursor = "default";
                break;
        }
        startDrag = null;
        endDrag = null;
    }
    return false;
}

/*Sets to point within the bounds of the control even if mouse is outside*/
function SharpMap_SetValid(p, obj) {
    if (p.x > obj.container.offsetWidth) p.x = obj.container.offsetWidth;
    if (p.x < 0) p.x = 0;
    if (p.y < 0) p.y = 0;
    if (p.y > obj.container.offsetHeight) p.y = obj.container.offsetHeight;
    return p;
}

var endDrag = null;
//var endMeasure = null;
function SharpMap_MapMouseOver(event, obj) {
    if (SharpMap_IsDefined(startDrag)) {
        var e = event || window.event;
        endDrag = SharpMap_GetRelativePosition(e.clientX, e.clientY, obj.container);
        endDrag = SharpMap_SetValid(endDrag, obj);
        switch (obj.currTool) {
            case "pan":
                var dx = endDrag.x - startDrag.x;
                var dy = endDrag.y - startDrag.y;
                var img = obj.map1;
                if (obj.currMap == 2) img = obj.map2;
                img.style.left = dx + 'px';
                img.style.top = dy + 'px';
                obj.container.style.cursor = 'move';
                break;
            case "boxZoom":
                var w = parseFloat(endDrag.x) - parseFloat(startDrag.x);
                var h = parseFloat(endDrag.y) - parseFloat(startDrag.y);

                if (w < 0)
                    obj.zoomLayer.style.left = endDrag.x;

                obj.zoomLayer.style.width = Math.abs(w);

                if (h < 0)
                    obj.zoomLayer.style.top = endDrag.y;

                obj.zoomLayer.style.height = Math.abs(h);
                break;
            case "measure":
                SharpMap_Measure(obj, startDrag, endDrag);
                break;
        }
    }
    var position = WebForm_GetElementPosition(obj.container);
    var scale;
    var e = event || window.event;
    var pos = SharpMap_PixelToMap(e.clientX - position.x, e.clientY - position.y, obj);
    var round = Math.floor(-Math.log(obj.zoom / obj.container.offsetWidth));
    var zoom = obj.zoom;
    if (round > 0) {
        round = Math.pow(10, round);
        pos.x = Math.round(pos.x * round) / round;
        pos.y = Math.round(pos.y * round) / round;
        zoom = Math.round(zoom * round) / round;
    }
    else {
        pos.x = Math.round(pos.x);
        pos.y = Math.round(pos.y);
        zoom = Math.round(zoom);
    }
    obj.statusbar.innerHTML = '' + pos.x + ', ' + pos.y + ' - Map width=' + zoom + '';

}

/* Begins zooming around the point x,y */
function SharpMap_BeginZoom(obj, x, y, zoomval) {
    if (obj.zoomEnded == 0) return;
    obj.zoomEnded = 0;
    obj.container.style.cursor = 'wait';
    var position = WebForm_GetElementPosition(obj.container);
    var imgX = x - position.x;
    var imgY = y - position.y;
    if (obj.zoom / zoomval < obj.minZoom) zoomval = obj.zoom / obj.minZoom;
    if (obj.zoom / zoomval > obj.maxZoom) zoomval = obj.zoom / obj.maxZoom;
    var center = SharpMap_PixelToMap(imgX + (obj.container.offsetWidth * 0.5 - imgX) / zoomval, imgY + (obj.container.offsetHeight * 0.5 - imgY) / zoomval, obj);
    obj.zoom = obj.zoom / zoomval;
    obj.minX = center.x - obj.zoom * 0.5;
    obj.maxY = center.y + obj.zoom * obj.container.offsetHeight / obj.container.offsetWidth * 0.5;
    SharpMap_BeginRefreshMap(obj, 1); //Start refreshing the map while we're zooming
    obj.zoomEnded = 0;
    SharpMap_DynamicZoom((position.x - x) * (zoomval - 1), (position.y - y) * (zoomval - 1), zoomval, 0.0, obj);
}
/* Zoom to the original extents of the map */
function SharpMap_ZoomExtents(obj) {
    if (obj.zoomEnded == 0) return;
    obj.zoomEnded = 0;
    obj.container.style.cursor = 'wait';
    zoomval = obj.zoom / obj.maxZoom;
    obj.zoom = obj.maxZoom;
    obj.minX = obj.defMinX
    obj.maxY = obj.defMaxY
    SharpMap_BeginRefreshMap(obj, 1); //Start refreshing the map while we're zooming
    obj.zoomEnded = 0;
    SharpMap_DynamicZoom(obj.minX * zoomval, obj.maxY * zoomval, zoomval, 0.0, obj);
}

/*update the scale bar based on current view*/
function SharpMap_RedrawScaleBar(obj) {
    var scale = (obj.zoom / obj.container.offsetWidth) * metersToMiles;
    if (scale > 0.049) {
        obj.scalebar.style.width = 5 / scale;
        obj.scaletext.innerText = "5 mi";
    } else if (scale > 0.016) {
        obj.scalebar.style.width = 1 / scale;
        obj.scaletext.innerText = "1 mi";
    } else if (scale > 0.005) {
        obj.scalebar.style.width = 0.5 / scale;
        obj.scaletext.innerText = "0.5 mi";
    } else if (scale > 0.0018) {
        obj.scalebar.style.width = 0.1893939 / scale;
        obj.scaletext.innerText = "1000 ft";
    } else if (scale > 0.0006) {
        obj.scalebar.style.width = 0.09469697 / scale;
        obj.scaletext.innerText = "500 ft";
    } else if (scale > 0.0002) {
        obj.scalebar.style.width = 0.01893939 / scale;
        obj.scaletext.innerText = "100 ft";
    } else {
        obj.scalebar.style.width = 0.009469697 / scale;
        obj.scaletext.innerText = "50 ft";
    }
}

/* Recursive method started by SharpMap_BeginZoom */
function SharpMap_DynamicZoom(tox, toy, toscale, step, obj) {
    step = step + 0.2;
    var imgd = obj.VisibleMap();
    var width = Math.round(obj.container.offsetWidth * ((toscale - 1.0) * step + 1.0)) + 'px';
    var height = Math.round(obj.container.offsetHeight * ((toscale - 1.0) * step + 1.0)) + 'px';
    var left = Math.round(tox * step) + 'px';
    var top = Math.round(toy * step) + 'px';
    imgd.style.width = width;
    imgd.style.height = height;
    imgd.style.left = left;
    imgd.style.top = top;
    if (step < 0.99) {
        var delegate = function () { SharpMap_DynamicZoom(tox, toy, toscale, step, obj); };
        setTimeout(delegate, obj.zoomSpeed);
    }
    else {
        obj.zoomEnded = 1;
        if (obj.mapReady == 1) { SharpMap_BeginFade(obj); }
    }
    SharpMap_RedrawScaleBar(obj);
}
/* Starts the fading from one image to the other */
function SharpMap_BeginFade(obj) {
    obj.container.style.cursor = 'wait';
    var to = obj.HiddenMap();
    var from = obj.VisibleMap();
    to.style.zIndex = 10;
    from.style.zIndex = 9;
    to.style.width = '';
    to.style.height = '';
    to.style.left = '';
    to.style.top = '';
    from.onload = ''; //Clear the onload event
    SharpMap_SetOpacity(to, 0);
    to.style.visibility = 'visible';
    if (obj.onViewChange)
        obj.onViewChange();
    if (obj.currMap == 2) { obj.currMap = 1; } else { obj.currMap = 2; }
    SharpMap_Fade(20, 20, from, to, obj);
}

/* Recursive method started from SharpMap_BeginFade */
function SharpMap_Fade(value, step, from, to, obj) {
    SharpMap_SetOpacity(to, value);
    if (value < 100) {
        var delegate = function () { SharpMap_Fade((value + step), step, from, to, obj); };
        setTimeout(delegate, obj.fadeSpeed);
    }
    else {
        from.style.visibility = 'hidden';
        obj.container.style.cursor = 'auto';
    }
}

/* Resize handle and method of responding to window/map resizing */
var resizeHandle;
function SharpMap_ResizeTimeout(event, obj) {
    /*
    if (resizeHandle!=0) { clearTimeout(resizeHandle); }
    var delegate = function() { SharpMap_BeginRefreshMap(obj,1); };
    resizeHandle = setTimeout(delegate,500);
    */
}
/* Requests a new map from the server using async callback and starts fading when the image have been retrieved*/
function SharpMap_BeginRefreshMap(obj, dofade) {
    var center = SharpMap_GetCenter(obj);
    var delegate = function (url) { SharpMap_GetCallbackResponse(url, obj, dofade); };
    WebForm_DoCallback(obj.container.id, center.x + ';' + center.y + ';' + obj.zoom + ';' + obj.container.offsetWidth + ';' + obj.container.offsetHeight + ';' + obj.hiddenLayers, delegate, null, SharpMap_AjaxOnError, true)
    obj.mapReady = 0;
    obj.container.style.cursor = 'wait';
    if (obj.onViewChanging)
        obj.onViewChanging();
}
/* Processes the response from the callback -
The function sets up an onload-even for when the image should start fading if dofade==1 */
function SharpMap_GetCallbackResponse(url, obj, dofade) {
    if (url == '') return;
    if (dofade == 1) {
        var imgdnew = obj.HiddenMap();
        imgdnew.src = url;
        imgdnew.onload = function () { obj.mapReady = 1; imgdnew.onload = ''; if (obj.zoomEnded == 1) { SharpMap_BeginFade(obj); } }
    }
    else {
        obj.VisibleMap().src = url;
        obj.container.style.cursor = 'auto';
        obj.VisibleMap().onload = function () { obj.mapReady = 1; }
    }
}
/* Returns the center of the current view */
function SharpMap_GetCenter(obj) {
    var center = new Object();
    center.x = obj.minX + obj.zoom * 0.5;
    center.y = obj.maxY - obj.zoom * obj.container.offsetHeight / obj.container.offsetWidth * 0.5;
    return center;
}
/* Sets the opacity of an object (x-browser) */
function SharpMap_SetOpacity(obj, value) {
    obj.style.opacity = value / 100.0;
    obj.style.mozopacity = value / 100.0;
    obj.style.filter = 'ALPHA(opacity=' + value + ')';
}
function SharpMap_AjaxOnError() { alert('Map refresh failed: ' + arg); }
/* Transforms from pixels coordinates to world coordinates */
function SharpMap_PixelToMap(x, y, obj) {
    var p = new Object();
    p.x = obj.minX + x * obj.zoom / obj.container.offsetWidth;
    p.y = obj.maxY - y * obj.zoom / obj.container.offsetWidth;
    return p;
}
/* Returns the relative position of a point to an object */
function SharpMap_GetRelativePosition(x, y, obj) {
    var position = WebForm_GetElementPosition(obj);
    var p = new Object();
    p.x = x - position.x;
    p.y = y - position.y;
    return p;
}
function SharpMap_IsDefined(obj) {
    if (null == obj) { return false; }
    if ('undefined' == typeof (obj)) { return false; }
    return true;
}

function SharpMap_GetBoundingBox(obj, p1, p2) {
    var box = new Object();
    box.x1 = (p1.x < p2.x) ? p1.x : p2.x;
    box.x2 = (p2.x < p1.x) ? p1.x : p2.x;
    box.y1 = (p1.y < p2.y) ? p1.y : p2.y;
    box.y2 = (p2.y < p1.y) ? p1.y : p2.y;
    box.centerX = box.x1 + ((box.x2 - box.x1) / 2);
    box.centerY = box.y1 + ((box.y2 - box.y1) / 2);
    box.zoomAmount = (obj.container.offsetWidth / (box.x2 - box.x1) < obj.container.offsetHeight / (box.y2 - box.y1)) ? obj.container.offsetWidth / (box.x2 - box.x1) : obj.container.offsetHeight / (box.y2 - box.y1);
    return box;
}
function SharpMap_BoxBeginZoom(obj, x, y, zoomval) {
    if (obj.zoomEnded == 0) return;
    obj.zoomEnded = 0;
    obj.container.style.cursor = 'wait';
    if (obj.zoom / zoomval < obj.minZoom) zoomval = obj.zoom / obj.minZoom;
    if (obj.zoom / zoomval > obj.maxZoom) zoomval = obj.zoom / obj.maxZoom;
    var center = SharpMap_PixelToMap(x + (obj.container.offsetWidth * 0.5 - x) / zoomval, y + (obj.container.offsetHeight * 0.5 - y) / zoomval, obj);
    obj.zoom = obj.zoom / zoomval;
    obj.minX = center.x - obj.zoom * 0.5;
    obj.maxY = center.y + obj.zoom * obj.container.offsetHeight / obj.container.offsetWidth * 0.5;
    SharpMap_BeginRefreshMap(obj, 1); //Start refreshing the map while we're zooming
    obj.zoomEnded = 0;
    SharpMap_DynamicZoom(x * zoomval - 1, y * zoomval - 1, zoomval, 0.0, obj);
}
function SharpMap_Measure(obj, p1, p2) {

    jg.clear();
    jg.setColor("#ff0000"); // red
    jg.setStroke(2);

    jg.drawLine(p1.x, p1.y, p2.x, p2.y);
    jg.drawEllipse(p1.x - 2, p1.y - 2, 4, 4);
    jg.paint();
    
    obj.measure.style.visibility = "visible";
    obj.measure.style.top = p2.y;
    obj.measure.style.left = p2.x;

    var d1 = SharpMap_PixelToMap(p1.x, p1.y, obj);
    var d2 = SharpMap_PixelToMap(p2.x, p2.y, obj);

    /* */
    

    var dist = (Math.sqrt(Math.pow(d2.x - d1.x, 2) + Math.pow(d2.y - d1.y, 2))) *0.001;
    /*var dist = (Math.sqrt(Math.pow(d2.x - d1.x, 2) + Math.pow(d2.y - d1.y, 2))) * metersToMiles;*/

    if (dist > 1)
        obj.measure.innerText = roundNumber(dist, 5) + " km";
    else
        obj.measure.innerText = Math.round(1000 * dist) + " m";
}

function roundNumber(num, dec) {
    var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);
    return result;
}

function SharpMap_HiddenLayers(obj) {
    var getstr = "";
    var theForm = (!document.forms['Form1']) ? document.Form1 : document.forms['Form1'];
    var inputs = theForm.getElementsByTagName("INPUT");

    for (i = 0; i < inputs.length; i++)
        if (inputs[i].type == "checkbox" && inputs[i].checked == false) { getstr += inputs[i].id.substring(3, inputs[i].id.length) + ","; }

    if (getstr.length > 1) {
        getstr = getstr.substring(0, getstr.length - 1);
        obj.hiddenLayers = getstr;
    } else
        obj.hiddenLayers = "none";

    SharpMap_BeginRefreshMap(obj, 1);
}

/* disables members of the layergroup on Toggle*/
function togLayerGroup(cb, DivId) {
    var myDiv = document.getElementById("myDiv" + DivId);
    if (myDiv != null) {
        var inputs = myDiv.getElementsByTagName("INPUT");

        for (i = 0; i < inputs.length; i++)
            if (inputs[i].type == "checkbox") { inputs[i].disabled = !cb.checked; }
    }
}
/*toggle the toolbar*/
function togToolbar(toolId, DivId) {
    var myDiv = document.getElementById(DivId);
    if (myDiv != null) {
        var tools = myDiv.getElementsByTagName("img");

        for (i = 0; i < tools.length; i++)
            if (tools[i].id != toolId)
                tools[i].className = "button";
            else
                tools[i].className = "pressed";
    }
}
/*expand and collapse layergroup*/
function expandIt(id) {
    if (document.all) {
        whichEl = eval("myDiv" + id);
        whichIm = event.srcElement;
        if (whichEl.style.display == "none") {
            whichEl.style.display = "block";
            whichIm.src = "images/minus.gif";
        }
        else {
            whichEl.style.display = "none";
            whichIm.src = "images/plus.gif";
        }
    }
    else if (document.all)//LOOKUP on the website below for a netscape reference
    {
        whichEl = eval("document.myDiv" + id);

        whichIm = eval("document.images['img" + id + "']");
        if (whichEl.visibility == "hide") {
            whichEl.visibility = "show";
            whichIm.src = "images/minus.gif";
        }
        else {
            whichEl.visibility = "hide";
            whichIm.src = "images/plus.gif";
        }
        // arrange(); this needs more work for propper results in netscape
        //see http://www.webreference.com/dhtml/column12/outALLtwo.html
    }
}








 
