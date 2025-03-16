using UnityEngine;

namespace CustomFolder.Scripts.Ship
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipMover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _rotationOffset;

        private Rigidbody _rigidbody;
        private Vector3 _currentDirection;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(Vector2 inputDirection)
        {
            Vector3 targetDirection = transform.forward * _moveSpeed * inputDirection.magnitude;
            
            _currentDirection = Vector3.Slerp(_currentDirection, targetDirection, _acceleration * Time.fixedDeltaTime);
            _rigidbody.MovePosition(transform.position + transform.forward * (_currentDirection.magnitude * Time.fixedDeltaTime));
            
            Rotate(inputDirection);
        }

        private void Rotate(Vector3 inputDirection)
        {
            if (inputDirection.magnitude > 0f)
            {
                float angle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + _rotationOffset;
            
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, angle, 0), _rotateSpeed * Time.fixedDeltaTime);
            }
        }

        public void MoveToPoint(Vector3 point)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, point, _moveSpeed * Time.fixedDeltaTime);
            
            _rigidbody.MovePosition(nextPosition);
        }
        
        public void RotateToAngle(float angle, float speed)
        {
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speed * Time.fixedDeltaTime);
        }
    }
}