﻿using System;
using Runtime.Signal;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.UI
{
    public class GamePanelView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI moveCountText;
        
        private SignalBus _signalBus;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }
        
        private void SubscribeEvents()
        {
            _signalBus.Subscribe<UpdateCountOfRemainingMovesSignal>(OnUpdateMoveCount);
        }
        
        private void OnUpdateMoveCount(UpdateCountOfRemainingMovesSignal signal)
        {
            moveCountText.text = signal.MoveCount.ToString();
        }
        
        private void UnsubscribeEvents()
        {
            _signalBus.Unsubscribe<UpdateCountOfRemainingMovesSignal>(OnUpdateMoveCount);
        }
        
        private void OnDisable()
        {
            UnsubscribeEvents();
        }
    }
}