using System;
using System.Collections.Generic;

using RedRockStudio.SZD.Common.DamageData;

using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
    public class DebugBodyPart : IBodyPart
    {
        public event Action Damaged;

        private static readonly int ColorProperty = Shader.PropertyToID("_Color");

        private readonly IBodyPart _wraper;
        private readonly IEnumerable<Renderer> _renderers;
        private readonly string _name;
        private readonly IDamageable _damageable;
        private readonly float _maxHealth;

        private float _health;

        public Injuries Injuries => _wraper.Injuries;

        public DebugBodyPart(
                IBodyPart wraper,
                IEnumerable<Renderer> renderers,
                IDamageable damageable,
                float maxHealth,
                string name)
        {
            _wraper = wraper;
            _renderers = renderers;
            _name = name;
            _damageable = damageable;
            _maxHealth = maxHealth;
            _health = maxHealth;
        }

        public void Initialize()
        {
            _wraper.Initialize();
            _wraper.Damaged += OnDamaged;
            _damageable.Damaged += LogDamage;
        }

        public void Dispose()
        {
            _wraper.Initialize();
            _wraper.Damaged -= OnDamaged;
            _damageable.Damaged -= LogDamage;
        }

        public bool HasBeenHalved(bool reset) => _wraper.HasBeenHalved(reset);

        public bool HaveBeenKilled(bool reset) => _wraper.HaveBeenKilled(reset);

        public bool HaveAccumulated(int damage, bool reset)
        {
            bool result = _wraper.HaveAccumulated(damage, reset);
            //Debug.Log($"{_name} is {result} for accumulated {damage} damage");
            return result;
        }

        private void OnDamaged() => Damaged?.Invoke();

        private void LogDamage(DamageData damageData)
        {
            _health -= damageData.Value;

            foreach (Renderer renderer in _renderers)
            {
                foreach (Material material in renderer.materials)
                    if (material.HasColor(ColorProperty))
                        material.SetColor(
                                          ColorProperty, Color.Lerp(Color.red, Color.white, _health / _maxHealth));
            }

            Debug.Log($"{_name} damaged: {damageData}");
        }
    }
}