using System.ComponentModel;
using System.Configuration.Install;

namespace _13_CustomAction
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            var secretsInstaller = new SecretsInstaller();
            Installers.Add(secretsInstaller);
        }
    }
}