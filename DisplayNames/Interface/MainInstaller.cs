using ComputerInterface.Interfaces;
using Zenject;

namespace DisplayNames.Interface
{
    internal class MainInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<IComputerModEntry>().To<Views.SetNameView.Entry>().AsSingle();
        }
    }
}
