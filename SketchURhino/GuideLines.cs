using Rhino.Geometry;
using System.Drawing;

namespace SketchURhino;

[CommandStyle(Style.ScriptRunner)]
public class SurGuideLines : Command
{
    public SurGuideLines()
    {
        // Rhino only creates one instance of each command class defined in a
        // plug-in, so it is safe to store a refence in a static property.
        Instance = this;

    }

    ///<summary>The only instance of this command.</summary>
    public static SurGuideLines Instance { get; private set; }

    ///<returns>The command name as it appears on the Rhino command line.</returns>
    public override string EnglishName => "SURGuideLines";

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
        // TODO: start here modifying the behaviour of your command.
        AddLayer(doc);

        GetPoint gp = new();
        gp.SetCommandPrompt("Point on curve to add knot");
        gp.Get();

        Point3d pt = gp.Point();
        var osnapofPt = gp.OsnapEventType;
        //if osnapofPt is"End" or "Intersection", then it is a point,else line
        if (gp.CommandResult() != Result.Success) return Result.Failure;
        var crv = gp.PointOnCurve(out _);
        var onSurface = gp.PointOnSurface(out _, out _);
        //var onBrep = gp.PointOnBrep(out _, out _);
        //var onObject = gp.PointOnObject();
        if (onSurface is Extrusion extrusion)
        {
            var curves = extrusion.ToBrep().Edges;
            foreach (var curve in curves)
            {
                if (!curve.IsLinear()) continue;
                curve.TryGetPolyline(out var polyline);
                var ptOnEdge = polyline.ClosestPoint(pt);
                //doc.Objects.AddPoint(ptOnEdge);
                //doc.Objects.AddPolyline(polyline);
                var distance = pt.DistanceTo(ptOnEdge);
                if (!(distance < _tolerance)) continue;
                OffsetSelLine(doc, polyline.SegmentAt(0));
            }
        }
        //有线、不捕捉端点和交点，则偏移线
        else if (crv != null && osnapofPt != OsnapModes.End && osnapofPt != OsnapModes.Intersection)
        {
            //内插线
            Line line = new();
            //多段线
            if (crv.IsPolyline())
            {
                crv.TryGetPolyline(out var poly);
                int segmentIndex =SegmentIndex(poly,pt);
                line = poly.SegmentAt(segmentIndex);
            }
            //多段线带弧形
            var type = crv.GetType().ToString();
            if (type == "Rhino.Geometry.PolyCurve")
            {
                if (crv is PolyCurve polyCurve)
                {
                    int segmentIndex = SegmentIndex(polyCurve, pt);
                    var segment = polyCurve.SegmentCurve(segmentIndex);
                    //如果是直线
                    if (segment.IsLinear())
                    {
                        //curve to line
                        if (segment is LineCurve lineCurve)
                        {
                            line = lineCurve.Line;
                        }
                            
                    }
                }
                    
            }

            if (line.Length >= _tolerance)
            {
                OffsetSelLine(doc, line);
                return Result.Success;  
            }
            else
            {
                GuideByPt(doc, pt);
                return Result.Success;
            }
                
      
        }
        else
        {
            //选择了点
            //get a pt
            GuideByPt(doc, pt);
            return Result.Success;
        }
        return Result.Failure;
    }

    


    //新建图层
    internal static void AddLayer(RhinoDoc doc, string layerName = GuidesLayerName)
    {
        
        var layerTable = doc.Layers;
        var layer = layerTable.FindName(layerName);
        //如果不存在
        if (layer == null)
        {
            var newLayer = new Layer(){Name = layerName ,IsLocked = false,IsVisible = true};
            // Create new layer
            newLayer.Color = Color.LightGray;
            var linetypeTable = doc.Linetypes;
            var linetype = linetypeTable.FindName("Hidden"); // 通过名称
            newLayer.LinetypeIndex = linetype.Index;
            layerTable.Add(newLayer);
        }
        else
        {
            if (layer.IsLocked || !layer.IsVisible)
            {
                layer.IsLocked = false;
                layer.IsVisible = true;
            }
        }
    }

    //closet segment of polycurve
    private int SegmentIndex(PolyCurve polyCurve,Point3d point3d)
    {
        
        if (polyCurve == null)
            return -1;
        var segmentCount = polyCurve.SegmentCount;
        if (segmentCount == 0)
            return -1;
        var segmentIndex = -1;
        for (var i = 0; i < segmentCount; i++)
        {
            var segment = polyCurve.SegmentCurve(i);
            segment.ClosestPoint(point3d, out double tOnCrv);
            var segmentClosestPoint = segment.PointAt(tOnCrv);
            double distance = point3d.DistanceTo(segmentClosestPoint);
            if (distance < _tolerance)
            {
                segmentIndex = i;
                return segmentIndex;
            }
        }
        return segmentIndex;
    }

    //closet segment of polyline
    private int SegmentIndex(Polyline polyline, Point3d point3d)
    {
        if (polyline == null)
            return -1;
        var segmentCount = polyline.SegmentCount;
        if (segmentCount == 0)
            return -1;
        var segmentIndex = -1;
        //var segmentParameterMin = double.MaxValue;
        for (var i = 0; i < segmentCount; i++)
        {
            var segment = polyline.SegmentAt(i);
            var segmentClosestPoint = segment.ClosestPoint(point3d, true);
            double distance = point3d.DistanceTo(segmentClosestPoint);
            if (distance < _tolerance)
            {
                segmentIndex = i;
                return segmentIndex;
            }
        }
        return segmentIndex;
    }

    private static bool RunScript()
    {
        string commandString = "_Offset _OutputLayer=_Input _ThroughPoint";
        // Call the command
        var run =RhinoApp.RunScript(commandString, true);
        if (run)
        {
            commandString = "_Noecho _SelLast";
            // Call the command
            run = RhinoApp.RunScript(commandString, true);
        }
        return run;
    }



    private Result OffsetSelLine(RhinoDoc doc, Line line)
    {
        ObjectAttributes attributes = new() { LayerIndex = doc.Layers.FindName(GuidesLayerName).Index };
        var newline = doc.Objects.AddLine(line, attributes);
        if (newline != Guid.Empty)
        {
            //change the line to new layer
            var refLine = _objectTable.Find(newline);
            //偏移
            doc.Objects.UnselectAll();
            var lineref = new ObjRef(refLine);
            doc.Objects.Select(lineref);
            if (RunScript())
            {
                doc.Objects.Delete(refLine, true);
                var selectedObjects = doc.Objects.GetSelectedObjects(false, false);
                var offsetLine = selectedObjects.Last();
                if (offsetLine.Geometry is Curve curve)
                {
                    // Extend the curve in both directions
                    curve = ExtendCurve(curve);
                    doc.Objects.AddCurve(curve, attributes);
                    doc.Objects.Delete(offsetLine, true);
                    doc.Views.Redraw();
                    return Result.Success;
                }
            }
        }
        return Result.Failure;
    }

    private Result GuideByPt(RhinoDoc doc ,Point3d pt)
    {
        GetPoint gp = new();
        gp.SetCommandPrompt("End of line");
        gp.SetBasePoint(pt, false);
        gp.DrawLineFromPoint(pt, true);
        gp.Get();
        if (gp.CommandResult() != Result.Success)
            return gp.CommandResult();
        Point3d ptEnd = gp.Point();
        Vector3d v = ptEnd - pt;
        if (v.IsTiny(RhinoMath.ZeroTolerance))
            return Result.Nothing;
        ObjectAttributes attributes = new();
        attributes.LayerIndex = doc.Layers.FindName(GuidesLayerName).Index;
        //doc.Objects.AddPoint(pt,attributes);
        //doc.Objects.AddPoint(pt_end, attributes);
        // Get the curve to extend
        Curve curve = new LineCurve(pt, ptEnd);
        var distance = curve.GetLength();
        RhinoApp.WriteLine("Distance:{0}", distance);
        // Extend the curve in both directions
        curve = ExtendCurve(curve);
        var newline = doc.Objects.AddCurve(curve, attributes);
        if (newline != Guid.Empty)
        {
            doc.Views.Redraw();
            return Result.Success;
        }
        return Result.Success;
    }

    private Curve ExtendCurve(Curve curve)
    {
        curve = curve.Extend(CurveEnd.Both, curve.GetLength() < 200000.0 ? (200000.0 - curve.GetLength() / 2) : 100, CurveExtensionStyle.Line);
        return curve;
    }

    private readonly double _tolerance = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;
    internal const string GuidesLayerName = "_Guides";
    private readonly ObjectTable _objectTable = RhinoDoc.ActiveDoc.Objects;
}