using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
    public class AttackBehaviour : MonoBehaviour
    {
        private readonly Collider2D[ ] _colliders = new Collider2D[5];

        [SerializeField]
        private LayerMask _layers;

        [SerializeField]
        private float _radius;

        [SerializeField]
        private Vector2 _position;

        public void Kick()
        {
            int count = Physics2D.OverlapCircleNonAlloc(
                                                        transform.TransformPoint(_position), _radius, _colliders,
                                                        _layers);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var attackedEvent = _colliders[i].GetComponentInParent<AttackedEvent>();

                    if (attackedEvent != null)
                    {
                        attackedEvent.Kick();
                        return;
                    }
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.TransformPoint(_position), _radius);
        }
#endif
    }
}