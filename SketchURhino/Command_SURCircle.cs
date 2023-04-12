using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Rhino.Runtime;

[CommandStyle(Style.ScriptRunner)]
public class Command_SURCircle : Command
{
  public override string EnglishName
  {
    get { return "SURCircle"; }
  }

  private PythonScript m_script;
  private PythonCompiledCode m_compiledCode;
  protected override Result RunCommand(RhinoDoc doc, RunMode mode)
  {
    CompilerPlugin.LoadLibraries();

    if (m_compiledCode == null)
    {
      ResourceManager rm = new ResourceManager("SketchURhino.EmbeddedResources.ScriptCode",
                                                Assembly.GetExecutingAssembly());
      string source = rm.GetString("SURCircle");
      source = DecryptString(source);
      m_script = PythonScript.Create();
      m_compiledCode = m_script.Compile(source);
    }

    if (m_compiledCode == null)
    {
      RhinoApp.WriteLine("The script code for {0} could not be retrieved or compiled.", EnglishName);
      return Result.Failure;
    }
    
    m_script.ScriptContextDoc = doc;
    m_script.SetVariable("__name__", "__main__");

    m_compiledCode.Execute(m_script);
    return Result.Success;
  }

  private string DecryptString(string text)
  {
    if (text == null) { throw new ArgumentNullException("text"); }
    if (text.Length == 0) { return string.Empty; }

    byte[] data = Convert.FromBase64String(text);

    RijndaelManaged rijndael = new RijndaelManaged();
    rijndael.KeySize = 128;
    rijndael.BlockSize = 128;

    Guid key = new Guid("41ef7883-f379-beaa-7944-b066bb51390c");
    rijndael.Key = key.ToByteArray();
    rijndael.IV = key.ToByteArray();
    rijndael.Mode = CipherMode.CBC;
    rijndael.Padding = PaddingMode.PKCS7;

    ICryptoTransform decryptor = rijndael.CreateDecryptor();
    byte[] result = decryptor.TransformFinalBlock(data, 0, data.Length);

    return Encoding.UTF8.GetString(result);
  }
}
