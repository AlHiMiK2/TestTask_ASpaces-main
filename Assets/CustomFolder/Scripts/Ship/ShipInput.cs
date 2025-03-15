﻿using CustomFolder.Scripts.UI;
using UnityEngine;

namespace CustomFolder.Scripts.Ship
{
    public class ShipInput : MonoBehaviour
    {
        private UIHandler _ui;
        
        private void Start()
        {
            _ui = UIHandler.Instance;
        }

        public Vector2 GetMoveDirection() => _ui.MoveJoystick.Direction;

        public Vector2 GetLookDirection() => _ui.LookJoystick.Direction;
    }
}