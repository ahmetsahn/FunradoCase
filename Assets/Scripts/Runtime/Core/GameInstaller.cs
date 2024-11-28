using Runtime.Managers;
using Runtime.Signal;
using Runtime.UI.Manager;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Runtime.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelManagerConfig levelManagerConfig;
        
        [SerializeField]
        private UIManagerConfig uiManagerConfig;
        
        public override void InstallBindings()
        {
            BindSignals();
            BindServices();
        }
        
        private void BindServices()
        {
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().WithArguments(levelManagerConfig);
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
            Container.BindInterfacesTo<LevelLoader>().AsSingle();
            Container.BindInterfacesTo<UIManager>().AsSingle().WithArguments(uiManagerConfig);
            
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle();
        }
        
        private void BindSignals()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<LoadLevelSignal>();
            Container.DeclareSignal<CompleteLevelSignal>();
            Container.DeclareSignal<GameOverSignal>();
            Container.DeclareSignal<OpenUIPanelSignal>();
            Container.DeclareSignal<CloseUIPanelSignal>();
            Container.DeclareSignal<CloseAllUIPanelsSignal>();
            Container.DeclareSignal<UpdateCountOfRemainingMovesSignal>();
            Container.DeclareSignal<ReduceCountOfRemainingFrogSignal>();
            Container.DeclareSignal<DestroyCurrentLevelSignal>();
        }
    }
}