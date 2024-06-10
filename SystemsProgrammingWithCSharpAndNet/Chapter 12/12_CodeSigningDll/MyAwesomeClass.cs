using _12_CodeSigningInterfaces;
using ExtensionLibrary;

namespace _12_CodeSigningDll
{
    public class MyAwesomeClass : IMyAwesomeClass
    {
        public void DoSomething()
        {
            "Hello from MyAwesomeClass!".Dump();
        }
    }
}
