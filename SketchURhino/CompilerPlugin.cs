public class CompilerPlugin : PlugIn
{
    private static bool librariesLoaded;
    internal static void LoadLibraries()
    {
        if (librariesLoaded)
            return;
        librariesLoaded = true;


    }

    protected override LoadReturnCode OnLoad(ref string errorMessage)
    {
        var result = base.OnLoad(ref errorMessage);
        string message = "meow";
        if (!string.IsNullOrWhiteSpace(message))
        {
            RhinoApp.WriteLine(message);
        }
        return result;
    }
}