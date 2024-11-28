using Runtime.Managers;
using Runtime.Signal;
using Runtime.UI.Manager;
using UnityEngine;
using Zenject;

namespace Runtime.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private UIManagerConfig uiManagerConfig;
        
        public override void InstallBindings()
        {
            BindSignals();
            BindServices();
        }
        
        private void BindServices()
        {
            Container.Bind<LevelManager>().AsSingle();
            
            Container.BindInterfacesTo<GameManager>().AsSingle();
            Container.BindInterfacesTo<LevelLoader>().AsSingle();
            Container.BindInterfacesTo<UIManager>().AsSingle().WithArguments(uiManagerConfig);
            
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle();
        }
        
        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<LoadLevelSignal>();
            Container.DeclareSignal<CompleteLevelSignal>();
            Container.DeclareSignal<RetryLevelSignal>();
            Container.DeclareSignal<OpenUIPanelSignal>();
            Container.DeclareSignal<CloseUIPanelSignal>();
            Container.DeclareSignal<CloseAllUIPanelsSignal>();
        }
    }
}