using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Rhino.Runtime;

[CommandStyle(Style.ScriptRunner)]
public class Command_SURExtrudeSurface : Command
{
  public override string EnglishName
  {
    get { return "SURExtrudeSurface"; }
  }

  protected override Result RunCommand(RhinoDoc doc, RunMode mode)
  {
      string commandString = "! _ExtrudeSrf _Pause _DeleteInput=_Yes _Solid=_Yes _Pause";
      // Call the command
      var run = RhinoApp.RunScript(commandString, true);
      return run ? Result.Success : Result.Failure;
  }

  //private PythonScript m_script;
  //private PythonCompiledCode m_compiledCode;
  //protected override Result RunCommand(RhinoDoc doc, RunMode mode)
  //{
  //  CompilerPlugin.LoadLibraries();

    //  if (m_compiledCode == null)
    //  {
    //    ResourceManager rm = new ResourceManager("SketchURhino.EmbeddedResources.ScriptCode",
    //                                              Assembly.GetExecutingAssembly());
    //    string source = rm.GetString("SURExtrudeSurface");
    //    source = DecryptString(source);
    //    m_script = PythonScript.Create();
    //    m_compiledCode = m_script.Compile(source);
    //  }

    //  if (m_compiledCode == null)
    //  {
    //    RhinoApp.WriteLine("The script code for {0} could not be retrieved or compiled.", EnglishName);
    //    return Result.Failure;
    //  }

    //  m_script.ScriptContextDoc = doc;
    //  m_script.SetVariable("__name__", "__main__");

    //  m_compiledCode.Execute(m_script);
    //  return Result.Success;
    //}

    //private string DecryptString(string text)
    //{
    //  if (text == null) { throw new ArgumentNullException("text"); }
    //  if (text.Length == 0) { return string.Empty; }

    //  byte[] data = Convert.FromBase64String(text);

    //  RijndaelManaged rijndael = new RijndaelManaged();
    //  rijndael.KeySize = 128;
    //  rijndael.BlockSize = 128;

    //  Guid key = new Guid("6336782c-4841-0de0-5c95-8f2b70fd8a48");
    //  rijndael.Key = key.ToByteArray();
    //  rijndael.IV = key.ToByteArray();
    //  rijndael.Mode = CipherMode.CBC;
    //  rijndael.Padding = PaddingMode.PKCS7;

    //  ICryptoTransform decryptor = rijndael.CreateDecryptor();
    //  byte[] result = decryptor.TransformFinalBlock(data, 0, data.Length);

    //  return Encoding.UTF8.GetString(result);
    //}
}
