using UnityEngine;

namespace CustomFolder.Scripts.Weapon
{
    [RequireComponent(typeof(WeaponFollow))]
    public class Weapon : MonoBehaviour
    {
        private WeaponFollow _weaponFollow;

        private void Awake()
        {
            _weaponFollow = GetComponent<WeaponFollow>();
        }

        public void SetOwner(Transform owner)
        {
            _weaponFollow.SetTarget(owner);
        }
    }
}
