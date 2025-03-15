using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _transitionSpeed;

    private bool _isActive = true;
    private bool _isTransition;
    
    private void LateUpdate()
    {
        if (_isActive && !_isTransition && _target)
        {
            transform.position = _target.position + _offset;
        }
    }

    public void SetActive(bool active)
    {
        _isActive = active;
    }
    
    public void SetTarget(Transform target)
    {
        _target = target;
        StartCoroutine(Transit());
    }

    private IEnumerator Transit()
    {
        _isTransition = true;
        
        while (Vector3.Distance(transform.position, _target.position + _offset) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target.position + _offset, _transitionSpeed * Time.deltaTime);
            
            yield return null;
        }

        _isTransition = false;
    }

    public void SetOffset(Vector3 offset)
    {
        _offset = offset;
    }
}
