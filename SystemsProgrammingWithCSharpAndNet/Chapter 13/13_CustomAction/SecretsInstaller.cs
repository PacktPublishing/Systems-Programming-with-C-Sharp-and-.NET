using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;

namespace _13_CustomAction
{
    [RunInstaller(true)]
    public partial class SecretsInstaller : Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            var secret = Guid.NewGuid().ToString();

            var targetDir = 
                Context.Parameters["targetdir"];
            var appSettingsPath = 
                Path.Combine(targetDir, "appsettings.json");
            if (File.Exists(appSettingsPath))
            {
                var appSettingsContent = 
                    File.ReadAllText(appSettingsPath);
                appSettingsContent = 
                    appSettingsContent.Replace(
                        "SECRET_PLACEHOLDER", 
                        secret);
                File.WriteAllText(
                    appSettingsPath, 
                    appSettingsContent);
            }
        }
    }
}