using UnityEngine;

namespace CustomFolder.Scripts.Ship
{
    public class TakeoffTrigger : MonoBehaviour
    {
        [SerializeField] private Ship _targetShip;
        [SerializeField] private LandingTrigger _landingTrigger;
        [SerializeField] private Transform _shipPoint;
        [SerializeField] private Transform _leaveShipPoint;
        
        public LandingTrigger LandingTrigger => _landingTrigger;
        public Transform LeaveShipPoint => _leaveShipPoint;
        public Transform ShipPoint => _shipPoint;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player.Player player))
            {
                player.SeatToShip(this, _targetShip);
                gameObject.SetActive(false);
            }
        }

        public void SetTargetShip(Ship ship)
        {
            _targetShip = ship;
        }
    }
}