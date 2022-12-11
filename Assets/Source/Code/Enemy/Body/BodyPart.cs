using System;

using RedRockStudio.SZD.Common.DamageData;

using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
    public class BodyPart : IBodyPart
    {
        private const int ForceCoefficient = 100;
        private const float Half = 0.5f;
        private const float DeathThreshold = 0.01f;
        private const int MaxForce = 800;

        public event Action Damaged;

        private readonly Rigidbody2D _rigidbody;
        private readonly IDamageable _damageable;
        private readonly float _maxHealth;

        private int _health;
        private bool _halved;
        private bool _dead;
        private int _accumulatedDamage;

        public Injuries Injuries =>
                ((_maxHealth - _health) / _maxHealth) switch
                {
                        <= 0.1f => Injuries.None,
                        <= 0.5f => Injuries.Low,
                        <= 0.95f => Injuries.Medium,
                        _ => Injuries.Full,
                };

        public BodyPart(Rigidbody2D rigidbody, IDamageable damageable, int maxHealth)
        {
            _rigidbody = rigidbody;
            _damageable = damageable;
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        public void Initialize() => _damageable.Damaged += ProcessDamage;

        public void Dispose() => _damageable.Damaged -= ProcessDamage;

        public bool HasBeenHalved(bool reset)
        {
            if (!_halved && _health / _maxHealth <= Half)
            {
                if (reset)
                    _halved = true;
                return true;
            }

            return false;
        }

        public bool HaveBeenKilled(bool reset)
        {
            if (!_dead && _health <= DeathThreshold)
            {
                if (reset)
                    _dead = true;
                return true;
            }

            return false;
        }

        public bool HaveAccumulated(int damage, bool reset)
        {
            if (_accumulatedDamage >= damage)
            {
                if (reset)
                    _accumulatedDamage = 0;
                return true;
            }

            return false;
        }

        private void ProcessDamage(DamageData data)
        {
            _accumulatedDamage += data.Value;
            _health -= data.Value;
            Damaged?.Invoke();
            Vector2 force = data.Direction.normalized * data.Value * ForceCoefficient;
            force = Vector2.ClampMagnitude(force, MaxForce);
            _rigidbody.AddForceAtPosition(force, data.Position);
        }
    }
}