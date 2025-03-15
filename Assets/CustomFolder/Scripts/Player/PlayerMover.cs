using UnityEngine;

namespace CustomFolder.Scripts.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private float _rotationOffset;

        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Move(Vector2 inputDirection)
        {
            Vector3 direction = new Vector3(inputDirection.x, 0f, inputDirection.y);
            
            direction = Quaternion.Euler(0, _rotationOffset, 0) * direction;
            
            _rigidbody.MovePosition(_rigidbody.position + direction * _moveSpeed * Time.fixedDeltaTime);
        }

        public void MoveToPoint(Vector3 point)
        {
            Vector3 nextPosition = Vector3.MoveTowards(transform.position, point, _moveSpeed * Time.fixedDeltaTime);
            
            _rigidbody.MovePosition(nextPosition);
        }

        public void Rotate(Vector2 inputDirection)
        {
            if (inputDirection.magnitude > 0f)
            {
                float angle = Mathf.Atan2(inputDirection.y, -inputDirection.x) * Mathf.Rad2Deg - _rotationOffset;
            
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), _rotateSpeed * Time.fixedDeltaTime);
            }
        }

        public void RotateToPoint(Vector3 point)
        {
            Vector3 direction = point - transform.position;
            
            if (direction.magnitude > 0f)
            {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, angle, 0), _rotateSpeed * Time.fixedDeltaTime);
            }
        }
    }
}