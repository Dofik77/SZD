using System;

using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
    public class DualBodyPart : IBodyPart
    {
        private readonly IBodyPart _first;
        private readonly IBodyPart _second;

        public event Action Damaged;

        public Injuries Injuries => CalculateInjuries();

        public DualBodyPart(IBodyPart first, IBodyPart second)
        {
            _first = first;
            _second = second;
        }

        public void Initialize()
        {
            _first.Initialize();
            _second.Initialize();
            _first.Damaged += OnDamaged;
            _second.Damaged += OnDamaged;
        }

        public void Dispose()
        {
            _first.Damaged -= OnDamaged;
            _second.Damaged -= OnDamaged;
            _first.Dispose();
            _second.Dispose();
        }

        public bool HasBeenHalved(bool reset) => _first.HasBeenHalved(reset) || _second.HasBeenHalved(reset);

        public bool HaveBeenKilled(bool reset) => _first.HaveBeenKilled(reset) || _second.HaveBeenKilled(reset);

        public bool HaveAccumulated(int damage, bool reset) =>
                _first.HaveAccumulated(damage, reset) || _second.HaveAccumulated(damage, reset);

        private void OnDamaged() => Damaged?.Invoke();

        private Injuries CalculateInjuries() => (Injuries)Mathf.Max((int)_first.Injuries, (int)_second.Injuries);
    }
}