using System.Collections;
using CustomFolder.Scripts.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CustomFolder.Scripts.Ship
{
    [RequireComponent(typeof(ShipMover))]
    [RequireComponent(typeof(ShipInput))]
    public class Ship : MonoBehaviour
    {
        [SerializeField] private Transform _enter;
        [SerializeField] private float _distanceToActivateLandingTrigger;
        [SerializeField] private Collider _collider;
        [SerializeField] private Vector3 _cameraOffset;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private float _takeoffDelay;
        [SerializeField] private float _landDelay;

        private ShipInput _input;
        private Player.Player _player;
        private ShipMover _mover;
        private bool _isControling;

        public event UnityAction OnReadyTakeoff;
        public event UnityAction OnReadyLand;
        public Transform Enter => _enter;

        private void Awake()
        {
            _mover = GetComponent<ShipMover>();
            _input = GetComponent<ShipInput>();
        }

        private void FixedUpdate()
        {
            if(_isControling)
                _mover.Move(_input.GetMoveDirection());
        }

        public void LandShip(LandingTrigger landingTrigger)
        {
            StartCoroutine(LandShipCoroutine(landingTrigger));
        }

        private IEnumerator LandShipCoroutine(LandingTrigger landingTrigger)
        {
            landingTrigger.gameObject.SetActive(false);
            _isControling = false;
            UIHandler.Instance.DisableJoysticks();
            DisableCollision();
            _cameraFollow.SetTarget(landingTrigger.TakeoffTrigger.LeaveShipPoint);
            _cameraFollow.SetOffset(_player.CameraOffset);
            
            Vector3 targetPosition = landingTrigger.TakeoffTrigger.ShipPoint.position;
            
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                _mover.MoveToPoint(targetPosition);
                _mover.RotateToPoint(targetPosition - landingTrigger.TakeoffTrigger.ShipPoint.forward * 10f);
                
                yield return new WaitForFixedUpdate();
            }
            
            OnReadyLand?.Invoke();
            
            yield return new WaitForSeconds(_landDelay);

            _player.transform.parent = null;
            _player.gameObject.SetActive(true);
            _player.LeaveShip(landingTrigger.TakeoffTrigger, this);
        }

        public void TakeoffShip(TakeoffTrigger takeoffTrigger, Player.Player player)
        {
            StartCoroutine(TakeoffShipCoroutine(takeoffTrigger, player));
        }

        private IEnumerator TakeoffShipCoroutine(TakeoffTrigger takeoffTrigger, Player.Player player)
        {
            _player = player;
            _player.transform.parent = transform;
            _player.gameObject.SetActive(false);
            _cameraFollow.SetActive(true);
            _cameraFollow.SetTarget(transform);
            _cameraFollow.SetOffset(_cameraOffset);

            OnReadyTakeoff?.Invoke();
            yield return new WaitForSeconds(_takeoffDelay);
            
            while (Vector3.Distance(takeoffTrigger.LandingTrigger.transform.position, transform.position) > 0.1f)
            {
                _mover.MoveToPoint(takeoffTrigger.LandingTrigger.transform.position);
                
                yield return new WaitForFixedUpdate();
            }
            
            _isControling = true;
            UIHandler.Instance.EnableJoysticks();
            EnableCollision();
            
            while (Vector3.Distance(takeoffTrigger.LandingTrigger.transform.position, transform.position) < _distanceToActivateLandingTrigger)
            {
                yield return null;
            }
            
            takeoffTrigger.LandingTrigger.gameObject.SetActive(true);
        }

        public void EnableCollision()
        {
            _collider.enabled = true;
        }

        public void DisableCollision()
        {
            _collider.enabled = false;
        }
    }
}