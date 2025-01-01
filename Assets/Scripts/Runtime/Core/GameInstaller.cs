using Ahmet.ObjectPool;
using Runtime.Managers;
using Runtime.Signal;
using Runtime.UI;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

namespace Runtime.Core
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private GameManagerConfig gameManagerConfig;
        
        [SerializeField]
        private LevelManagerConfig levelManagerConfig;
        
        [SerializeField]
        private LevelLoaderConfig levelLoaderConfig;
        
        [SerializeField]
        private UIManagerConfig uiManagerConfig;
        
        [SerializeField]
        private SoundManagerConfig soundManagerConfig;
        
        public override void InstallBindings()
        {
            BindSignals();
            BindServices();
        }
        
        private void BindServices()
        {
            Container.Bind<ObjectPoolManager>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle();
            
            Container.BindInterfacesTo<LevelLoader>().AsSingle().WithArguments(levelLoaderConfig);
            Container.BindInterfacesTo<UIManager>().AsSingle().WithArguments(uiManagerConfig);
            Container.BindInterfacesAndSelfTo<LevelManager>().AsSingle().WithArguments(levelManagerConfig);
            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle().WithArguments(gameManagerConfig);
            Container.BindInterfacesAndSelfTo<SoundManager>().AsSingle().WithArguments(soundManagerConfig);
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
            Container.DeclareSignal<DestroyCurrentLevelSignal>();
        }
    }
}