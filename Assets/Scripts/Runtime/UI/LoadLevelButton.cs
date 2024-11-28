using Runtime.Enums;
using Runtime.Managers;
using Runtime.Signal;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Runtime.UI
{
    public abstract class LoadLevelButton : MonoBehaviour
    {
        private Button _button;
        
        protected SignalBus SignalBus;
        
        private GameManager _gameManager;
        
        protected abstract void CloseUIPanel();
        
        [Inject]
        private void Construct(SignalBus signalBus, GameManager gameManager)
        {
            SignalBus = signalBus;
            _gameManager = gameManager;
        }
        
        private void Awake()
        {
            _button = GetComponent<Button>();
        }
        
        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _button.onClick.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            CloseUIPanel();
            SignalBus.Fire(new OpenUIPanelSignal(UIPanelType.GamePanel));
            _gameManager.LoadCurrentLevel();
        }
        
        private void UnsubscribeEvents()
        {
            _button.onClick.RemoveListener(OnClick);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}