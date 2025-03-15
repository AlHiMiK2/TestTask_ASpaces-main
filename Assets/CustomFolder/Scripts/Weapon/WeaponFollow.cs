using UnityEngine;

namespace CustomFolder.Scripts.Weapon
{
    public class WeaponFollow : MonoBehaviour
    {
        private Transform _target;

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        private void Update()
        {
            if (_target)
            {
                transform.SetPositionAndRotation(_target.position, _target.rotation);
            }
        }
    }
}