using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace _12_CodeSigningHost
{
    internal class SecureLoadContext : AssemblyLoadContext
    {
        private readonly string _expectedPublicKeyToken;

        public SecureLoadContext(string expectedPublicKeyToken)
        {
            _expectedPublicKeyToken = expectedPublicKeyToken;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            // Get the path to the assembly
            var currentPath = Assembly.GetExecutingAssembly().Location;
            
            var fullPath = Path.Combine(currentPath, $"{assemblyName}.dll" );


            //string assemblyPath = $"{assemblyName.Name}.dll";
            Assembly assembly = LoadFromAssemblyPath(fullPath);

            if (VerifyAssembly(assembly))
                return assembly;

            throw new SecurityException("Assembly Verification failed");

        }

        private bool VerifyAssembly(Assembly assembly)
        {
            byte[] publicKeyToken = assembly.GetName().GetPublicKeyToken();
            string publicKeyTokenString = BitConverter.ToString(publicKeyToken).Replace("-", "").ToLower();
            return _expectedPublicKeyToken.Equals(publicKeyTokenString, StringComparison.OrdinalIgnoreCase);
        }
    }
}
