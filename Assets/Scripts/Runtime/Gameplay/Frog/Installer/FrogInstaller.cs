using Runtime.Gameplay.Frog.Controller;
using Runtime.Gameplay.Frog.Model;
using Runtime.Gameplay.Frog.Model.Scriptable;
using Runtime.Gameplay.Frog.Service;
using Runtime.Gameplay.Frog.View;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Frog.Installer
{
    public class FrogInstaller : MonoInstaller
    {
        [SerializeField]
        private FrogAnimationControllerConfig frogAnimationControllerConfig;

        [SerializeField] 
        private SplineServiceConfig splineServiceConfig;
        
        [SerializeField]
        private FrogSo frogSo;
        
        public override void InstallBindings()
        {
            Container.Bind<FrogView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<FrogModel>().AsSingle().WithArguments(frogSo);
            Container.Bind<CollectablesService>().AsSingle();
            Container.Bind<RaycastService>().AsSingle();
            Container.Bind<SplineService>().AsSingle().WithArguments(splineServiceConfig);
            
            Container.BindInterfacesTo<FrogAnimationController>().AsSingle().WithArguments(frogAnimationControllerConfig);
            Container.BindInterfacesTo<FrogTongueController>().AsSingle();
            Container.BindInterfacesTo<FrogVisualController>().AsSingle();
        }
    }
}