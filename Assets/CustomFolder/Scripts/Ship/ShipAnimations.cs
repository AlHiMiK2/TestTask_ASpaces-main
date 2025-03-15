using UnityEngine;

namespace CustomFolder.Scripts.Ship
{
    [RequireComponent(typeof(Ship))]
    [RequireComponent(typeof(Animator))]
    public class ShipAnimations : MonoBehaviour
    {
        private Ship _ship;
        private Animator _animator;
        private static readonly int _takeoff = Animator.StringToHash("Takeoff");
        private static readonly int _land = Animator.StringToHash("Land");

        private void Awake()
        {
            _ship = GetComponent<Ship>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _ship.OnReadyTakeoff += StartTakeoffAnimations;
            _ship.OnReadyLand += StartLandAnimations;
        }

        private void OnDisable()
        {
            _ship.OnReadyTakeoff -= StartTakeoffAnimations;
            _ship.OnReadyLand -= StartLandAnimations;
        }

        private void StartTakeoffAnimations()
        {
            _animator.SetTrigger(_takeoff);
        }
        
        private void StartLandAnimations()
        {
            _animator.SetTrigger(_land);
        }
    }
}