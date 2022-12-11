using System;

using RedRockStudio.SZD.BehavioursInterfaces;

using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
    public class DetachableBodyPart : IBodyPart
    {
        public event Action Damaged;

        private readonly IDetachableBone _detachableBone;
        private readonly IBodyPart _bodyPartImplementation;

        public Injuries Injuries => _bodyPartImplementation.Injuries;

        public DetachableBodyPart(
                IDetachableBone detachableBone,
                Rigidbody2D rigidbody,
                IDamageable damageable,
                int maxHealth)
        {
            _bodyPartImplementation =
                    new BodyPart(rigidbody, damageable, maxHealth); //TODO maybe move to composition root
            _detachableBone = detachableBone;
        }

        public void Initialize()
        {
            _bodyPartImplementation.Initialize();
            _bodyPartImplementation.Damaged += OnDamaged;
        }

        public void Dispose()
        {
            _bodyPartImplementation.Damaged -= OnDamaged;
            _bodyPartImplementation.Dispose();
        }

        public bool HasBeenHalved(bool reset) => _bodyPartImplementation.HasBeenHalved(reset);

        public bool HaveBeenKilled(bool reset) => _bodyPartImplementation.HaveBeenKilled(reset);

        public bool HaveAccumulated(int damage, bool reset) => _bodyPartImplementation.HaveAccumulated(damage, reset);

        private void OnDamaged()
        {
            Damaged?.Invoke();
            if (Injuries == Injuries.Full)
                _detachableBone.Detach();
        }
    }
}