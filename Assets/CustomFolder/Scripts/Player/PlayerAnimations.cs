using UnityEngine;

namespace CustomFolder.Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Player))]
    public class PlayerAnimations : MonoBehaviour
    {
        [SerializeField] private Transform _spineBone;
        [SerializeField] private float _leftTurnAngle;
        [SerializeField] private float _rightTurnAngle;

        private float _spineAngle;
        private Animator _animator;
        private Player _player;
        private static readonly int _moveX = Animator.StringToHash("MoveX");
        private static readonly int _moveY = Animator.StringToHash("MoveY");
        private static readonly int _moveSpeed = Animator.StringToHash("MoveSpeed");
        private static readonly int _turnRight = Animator.StringToHash("TurnRight");
        private static readonly int _turnLeft = Animator.StringToHash("TurnLeft");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponent<Player>();
        }

        private void LateUpdate()
        {
            UpdateSpineAnimation(_player.MoveDirection, _player.LookDirection);
            UpdateTurnAnimation();
            UpdateMoveAnimation(_player.MoveDirection, _player.LookDirection);
        }

        private void UpdateSpineAnimation(Vector2 moveDirection, Vector2 lookDirection)
        {
            if (moveDirection.magnitude > 0f)
            {
                _spineAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            }
            else
            {
                if (lookDirection.magnitude > 0f)
                {
                    _spineAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
                }
                
                _spineBone.rotation = Quaternion.Euler(0f, 135f - _spineAngle, -90f);
            }
        }

        private void UpdateTurnAnimation()
        {
            if (_animator.GetFloat(_moveSpeed) == 0f)
            {
                float spineAngle = Vector3.SignedAngle(_spineBone.forward, transform.forward, Vector3.up);
                bool isLeft = spineAngle >= _leftTurnAngle;
                bool isRight = spineAngle <= _rightTurnAngle;
                
                _animator.SetBool(_turnLeft, isLeft);
                _animator.SetBool(_turnRight, isRight);
            }
            else
            {
                _animator.SetBool(_turnLeft, false);
                _animator.SetBool(_turnRight, false);
            }
        }

        public void LeftTurn()
        {
            transform.Rotate(0, -90f, 0);
        }

        public void RightTurn()
        {
            transform.Rotate(0, 90f, 0);
        }

        private void UpdateMoveAnimation(Vector2 moveDirection, Vector2 lookDirection)
        {
            if (lookDirection == Vector2.zero)
            {
                lookDirection = moveDirection;
            }
            
            Vector2 direction;
            Vector2 rotatedLookDirection = Quaternion.Euler(0, 0, 90) * lookDirection;
            direction.y = Vector2.Dot(moveDirection, lookDirection);
            direction.x = Vector2.Dot(moveDirection, rotatedLookDirection);
            direction.Normalize();
            
            _animator.SetFloat(_moveX, direction.x);
            _animator.SetFloat(_moveY, direction.y);
            _animator.SetFloat(_moveSpeed, moveDirection.magnitude);
        }
    }
}