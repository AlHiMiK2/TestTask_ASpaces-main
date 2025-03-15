using UnityEngine;

namespace CustomFolder.Scripts.Ship
{
    public class LandingTrigger : MonoBehaviour
    {
        [SerializeField] private TakeoffTrigger _takeoffTrigger;

        public TakeoffTrigger TakeoffTrigger => _takeoffTrigger;
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Ship ship))
            {
                ship.LandShip(this);
                _takeoffTrigger.SetTargetShip(ship);
            }
        }
    }
}