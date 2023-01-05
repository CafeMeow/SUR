[Rhino.Commands.CommandStyle(Rhino.Commands.Style.ScriptRunner)]
public class Command_SURExtrudeSurface : Rhino.Commands.Command
{
  public override string EnglishName
  {
    get { return "SURExtrudeSurface"; }
  }

  private Rhino.Runtime.PythonScript m_script;
  private Rhino.Runtime.PythonCompiledCode m_compiledCode;
  protected override Rhino.Commands.Result RunCommand(Rhino.RhinoDoc doc, Rhino.Commands.RunMode mode)
  {
    CompilerPlugin.LoadLibraries();

    if (m_compiledCode == null)
    {
      System.Resources.ResourceManager rm = new System.Resources.ResourceManager("SketchURhino.EmbeddedResources.ScriptCode",
                                                System.Reflection.Assembly.GetExecutingAssembly());
      string source = rm.GetString("SURExtrudeSurface");
      source = DecryptString(source);
      m_script = Rhino.Runtime.PythonScript.Create();
      m_compiledCode = m_script.Compile(source);
    }

    if (m_compiledCode == null)
    {
      Rhino.RhinoApp.WriteLine("The script code for {0} could not be retrieved or compiled.", EnglishName);
      return Rhino.Commands.Result.Failure;
    }
    
    m_script.ScriptContextDoc = doc;
    m_script.SetVariable("__name__", "__main__");

    m_compiledCode.Execute(m_script);
    return Rhino.Commands.Result.Success;
  }

  private string DecryptString(string text)
  {
    if (text == null) { throw new System.ArgumentNullException("text"); }
    if (text.Length == 0) { return string.Empty; }

    byte[] data = System.Convert.FromBase64String(text);

    System.Security.Cryptography.RijndaelManaged rijndael = new System.Security.Cryptography.RijndaelManaged();
    rijndael.KeySize = 128;
    rijndael.BlockSize = 128;

    System.Guid key = new System.Guid("6336782c-4841-0de0-5c95-8f2b70fd8a48");
    rijndael.Key = key.ToByteArray();
    rijndael.IV = key.ToByteArray();
    rijndael.Mode = System.Security.Cryptography.CipherMode.CBC;
    rijndael.Padding = System.Security.Cryptography.PaddingMode.PKCS7;

    System.Security.Cryptography.ICryptoTransform decryptor = rijndael.CreateDecryptor();
    byte[] result = decryptor.TransformFinalBlock(data, 0, data.Length);

    return System.Text.Encoding.UTF8.GetString(result);
  }
}
