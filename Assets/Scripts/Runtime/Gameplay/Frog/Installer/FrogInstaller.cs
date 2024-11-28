using Runtime.Gameplay.Frog.View;
using Zenject;

namespace Runtime.Gameplay.Frog.Installer
{
    public class FrogInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<FrogView>().FromComponentInHierarchy().AsSingle();
        }
    }
}