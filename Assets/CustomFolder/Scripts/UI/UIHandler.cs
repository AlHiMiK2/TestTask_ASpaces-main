using UnityEngine;

namespace CustomFolder.Scripts.UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Joystick _moveJoystick;
        [SerializeField] private Joystick _lookJoystick;

        public Joystick MoveJoystick => _moveJoystick; 
        public Joystick LookJoystick => _lookJoystick; 
        public static UIHandler Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        
        public void EnableJoysticks()
        {
            _moveJoystick.gameObject.SetActive(true);
            _lookJoystick.gameObject.SetActive(true);
            _moveJoystick.OnPointerUp(null);
            _lookJoystick.OnPointerUp(null);
        }

        public void DisableJoysticks()
        {            
            _moveJoystick.gameObject.SetActive(false);
            _lookJoystick.gameObject.SetActive(false);
            _moveJoystick.OnPointerUp(null);
            _lookJoystick.OnPointerUp(null);
        }
    }
}
