using UnityEngine;

namespace RedRockStudio.SZD.Components
{
    public class LaserSight : MonoBehaviour
    {
	    [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private ContactFilter2D _filter;
        
        private Transform _transform;

        private readonly RaycastHit2D[] _hits = new RaycastHit2D[10];
        private bool _isHitted;
        
        
        private void Start() => 
	        _transform = transform;

        private void FixedUpdate() => 
	        CastRay();

        private void CastRay()
        {
            int count = Physics2D.RaycastNonAlloc(
                transform.position, _transform.right, _hits, _maxDistance, _layerMask.value);
            
            if (count > 0)
                _lineRenderer.SetPosition(1, transform.InverseTransformPoint(_hits[0].point));
            else
                _lineRenderer.SetPosition(1, Vector3.right * _maxDistance);
        }
    }
}
