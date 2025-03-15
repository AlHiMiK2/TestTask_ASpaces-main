using System.Collections;
using CustomFolder.Scripts.Ship;
using CustomFolder.Scripts.UI;
using UnityEngine;

namespace CustomFolder.Scripts.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMover))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Weapon.Weapon _currentWeapon;
        [SerializeField] private Transform _weapBone;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private Vector3 _cameraOffset;
        
        private PlayerInput _input;
        private PlayerMover _mover;
        private bool _isControling = true;
        
        public Vector2 MoveDirection { get; private set; }
        public Vector2 LookDirection { get; private set; }
        public Vector3 CameraOffset => _cameraOffset;
        
        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _mover = GetComponent<PlayerMover>();
        }

        private void Start()
        {
            _currentWeapon.SetOwner(_weapBone);
            _cameraFollow.SetTarget(transform);
        }

        private void FixedUpdate()
        {
            if (_isControling)
            {
                MoveDirection = _input.GetMoveDirection();
                LookDirection = _input.GetLookDirection();
                Move(MoveDirection, LookDirection);
            }
        }

        private void Move(Vector2 moveDirection, Vector2 lookDirection)
        {
            if (moveDirection != Vector2.zero)
            {
                _mover.Move(moveDirection);
                
                if (lookDirection == Vector2.zero)
                    lookDirection = moveDirection;
                
                _mover.Rotate(lookDirection);
            }
        }

        public void SeatToShip(TakeoffTrigger takeoffTrigger, Ship.Ship ship)
        {
            StartCoroutine(SeatToShipCoroutine(takeoffTrigger, ship));
        }

        private IEnumerator SeatToShipCoroutine(TakeoffTrigger takeoffTrigger, Ship.Ship ship)
        {
            takeoffTrigger.gameObject.SetActive(false);
            takeoffTrigger.LandingTrigger.gameObject.SetActive(false);
            _isControling = false;
            UIHandler.Instance.DisableJoysticks();
            _cameraFollow.SetActive(false);
            ship.DisableCollision();

            Vector3 targetPosition = ship.Enter.position;
            targetPosition.y = transform.position.y;
            
            while (Vector3.Distance(targetPosition, transform.position) > 0.1f)
            {
                _mover.MoveToPoint(targetPosition);
                _mover.RotateToPoint(targetPosition);
                MoveDirection = Vector2.right;
                LookDirection = MoveDirection;
                
                yield return new WaitForFixedUpdate();
            }
            
            ship.TakeoffShip(takeoffTrigger, this);
        }
        
        public void LeaveShip(TakeoffTrigger takeoffTrigger, Ship.Ship ship)
        {
            StartCoroutine(LeaveShipCoroutine(takeoffTrigger, ship));
        }

        private IEnumerator LeaveShipCoroutine(TakeoffTrigger takeoffTrigger, Ship.Ship ship)
        {
            Vector3 targetPosition = takeoffTrigger.LeaveShipPoint.position;
            targetPosition.y = transform.position.y;
            
            while (Vector3.Distance(targetPosition, transform.position) > 0.1f)
            {
                _mover.MoveToPoint(targetPosition);
                _mover.RotateToPoint(targetPosition);
                MoveDirection = Vector2.left;
                LookDirection = MoveDirection;
                
                yield return new WaitForFixedUpdate();
            }

            _cameraFollow.SetTarget(transform);
            ship.EnableCollision();
            UIHandler.Instance.EnableJoysticks();
            _isControling = true;
            takeoffTrigger.gameObject.SetActive(true);
        }
    }
}