using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Runtime.Enums;
using Runtime.Signal;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Runtime.UI.Manager
{
    public class UIManager : IDisposable
    {
        private readonly SerializedDictionary<UIPanelType, GameObject> _panelPrefabs;
        private readonly SerializedDictionary<UIPanelType, Transform> _panelLayers;

        private readonly SignalBus _signalBus;
        
        private readonly IInstantiator _instantiator;

        public UIManager(
            SignalBus signalBus, 
            IInstantiator instantiator,
            UIManagerConfig config)
        {
            _signalBus = signalBus;
            _instantiator = instantiator;
            _panelPrefabs = config.PanelPrefabs;
            _panelLayers = config.PanelLayers;
            
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            _signalBus.Subscribe<OpenUIPanelSignal>(OnOpenPanel);
            _signalBus.Subscribe<CloseUIPanelSignal>(OnClosePanel);
            _signalBus.Subscribe<CloseAllUIPanelsSignal>(OnCloseAllPanels);
        }

        private void OnOpenPanel(OpenUIPanelSignal signal)
        {
            if (_panelPrefabs.TryGetValue(signal.PanelType, out var panelPrefab))
            {
                if (_panelLayers.TryGetValue(signal.PanelType, out var layer))
                {
                    _instantiator.InstantiatePrefab(panelPrefab, layer);
                }
                else
                {
                    Debug.LogError($"Layer for PanelType {signal.PanelType} not found.");
                }
            }
            else
            {
                Debug.LogError($"Prefab for PanelType {signal.PanelType} not found.");
            }
        }

        private void OnClosePanel(CloseUIPanelSignal signal)
        {
            if (_panelLayers.TryGetValue(signal.PanelType, out var layer) && layer.childCount > 0)
            {
                Object.Destroy(layer.GetChild(0).gameObject);
            }
        }

        private void OnCloseAllPanels()
        {
            foreach (var layer in _panelLayers.Values)
            {
                if (layer.childCount > 0)
                {
                    Object.Destroy(layer.GetChild(0).gameObject);
                }
            }
        }

        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<OpenUIPanelSignal>(OnOpenPanel);
            _signalBus.Unsubscribe<CloseUIPanelSignal>(OnClosePanel);
            _signalBus.Unsubscribe<CloseAllUIPanelsSignal>(OnCloseAllPanels);
        }

        public void Dispose()
        {
            UnsubscribeEvents();
        }
    }
    
    [Serializable]
    public struct UIManagerConfig
    {
        public SerializedDictionary<UIPanelType, GameObject> PanelPrefabs;
        public SerializedDictionary<UIPanelType, Transform> PanelLayers;
    }
}
