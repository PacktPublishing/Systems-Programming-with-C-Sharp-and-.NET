
using System.Reflection;
using _12_CodeSigningHost;
using _12_CodeSigningInterfaces;
using ExtensionLibrary;

string expectedPublicKeyToken = "9a647e2a27279d63";
var context = new SecureLoadContext(expectedPublicKeyToken);
var currentPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
var fullPath = Path.Combine(currentPath, "12_CodeSigningDll");
Assembly assembly = context.LoadFromAssemblyName(new AssemblyName("12_CodeSigningDll"));
//var myAwesomeClass = new MyAwesomeClass();
//myAwesomeClass.DoSomething();

var instance = (IMyAwesomeClass) assembly.CreateInstance("_12_CodeSigningDll.MyAwesomeClass");
instance.DoSomething();

//?.GetType().GetMethod("DoSomething")?.Invoke(null, null);

"Back in the original program.".Dump();
Console.ReadLine();