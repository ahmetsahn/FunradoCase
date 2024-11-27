using Runtime.Managers;
using Runtime.Signal;
using Zenject;

namespace Runtime.Core
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSignals();
            BindServices();
        }
        
        private void BindServices()
        {
            Container.BindInterfacesTo<GameManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle();
            Container.Bind<LevelManager>().AsSingle();
            Container.BindInterfacesTo<LevelLoader>().AsSingle();
        }
        
        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<LoadLevelSignal>();
            Container.DeclareSignal<CompleteLevelSignal>();
            Container.DeclareSignal<RetryLevelSignal>();
        }
    }
}