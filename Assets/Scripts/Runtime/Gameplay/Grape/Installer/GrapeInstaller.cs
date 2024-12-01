using Runtime.Gameplay.Grape.Controller;
using Runtime.Gameplay.Grape.Model;
using Runtime.Gameplay.Grape.Model.Scriptable;
using Runtime.Gameplay.Grape.View;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Grape.Installer
{
    public class GrapeInstaller : MonoInstaller
    {
        [SerializeField]
        private GrapeSo grapeSo;
        
        public override void InstallBindings()
        {
            Container.Bind<GrapeView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<GrapeModel>().AsSingle().WithArguments(grapeSo);
            
            Container.BindInterfacesTo<GrapeVisualController>().AsSingle();
        }
    }
}