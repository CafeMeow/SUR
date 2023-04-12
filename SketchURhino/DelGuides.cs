namespace SketchURhino;

public class SURDelGuides : Command
{
    public SURDelGuides()
    {
        // Rhino only creates one instance of each command class defined in a
        // plug-in, so it is safe to store a refence in a static property.
        Instance = this;

    }

    ///<summary>The only instance of this command.</summary>
    public static SURDelGuides Instance { get; private set; }

    ///<returns>The command name as it appears on the Rhino command line.</returns>
    public override string EnglishName => "SURDelGuides";

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
        // TODO: start here modifying the behaviour of your command.
        SurGuideLines.AddLayer(doc);
        // Get the objects in the layer
        RhinoObject[] rhobjs = doc.Objects.FindByLayer(SurGuideLines.GuidesLayerName);
        if (rhobjs == null || rhobjs.Length < 1)
            return Result.Cancel;

        foreach (var t in rhobjs)
            doc.Objects.Delete(t, true);

        doc.Views.Redraw();
        return Result.Success;
    }
}