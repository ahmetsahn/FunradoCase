﻿using System;
using Runtime.Enums;
using Runtime.Gameplay.Abstract;
using UnityEngine;

namespace Runtime.Gameplay.Cell.View
{
    public class CellView : ScalableObject
    {
        [SerializeField] 
        private GameObject childObject;
        
        [SerializeField]
        private bool isEmpty;

        protected override void Awake()
        {
            FindCellBelow();
        }
        
        private void SetActiveNewObject()
        {
            childObject.SetActive(true);
        }
        
        private bool IsEmpty()
        {
            return isEmpty;
        }
        
        private void OnDestroy()
        {
            if(CellViewBelow == null)
            {
                return;
            }
            
            if (!CellViewBelow.IsEmpty())
            {
                CellViewBelow.SetActiveNewObject();
            }
        }
    }
}