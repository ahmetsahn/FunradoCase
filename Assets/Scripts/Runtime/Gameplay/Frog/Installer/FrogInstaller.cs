using Runtime.Gameplay.Frog.Controller;
using Runtime.Gameplay.Frog.Model;
using Runtime.Gameplay.Frog.Service;
using Runtime.Gameplay.Frog.View;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Frog.Installer
{
    public class FrogInstaller : MonoInstaller
    {
        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private GameObject tongueSplinePrefab;
        
        [SerializeField]
        private FrogModelConfig frogModelConfig;
        
        public override void InstallBindings()
        {
            Container.Bind<FrogView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<FrogModel>().AsSingle().WithArguments(frogModelConfig);
            Container.BindInterfacesTo<FrogAnimationController>().AsSingle().WithArguments(animator);
            Container.BindInterfacesTo<FrogTongueController>().AsSingle();
            Container.Bind<CollectablesService>().AsSingle();
            Container.Bind<RaycastService>().AsSingle();
            Container.Bind<SplineService>().AsSingle().WithArguments(tongueSplinePrefab);
        }
    }
}