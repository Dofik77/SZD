using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MyBox;
using RedRockStudio.SZD.Enemy.Graphics;
using RedRockStudio.SZD.Enemy.HandsLogic;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
	public class IdleState : IEnemyState, IEnemyComponent
	{
		private const float Time = 1.5f;
		private const float KneelingTime = 0.15f;
		private const float KneelingAnimation = 0.667f;
		private const int DamageForKneeling = 5;

		public event Action Finished;

		private readonly IBody _body;
		private readonly IEnemyGraphics _graphics;
		private readonly IEnumerable<ILockable> _lockables;

		private float _timestamp;
		private bool _kneeled;
		private int _damageCount;
		private bool _isActive;
		
		public IdleState(IBody body, IEnemyGraphics graphics, IEnumerable<ILockable> lockables)
		{
			_body = body;
			_graphics = graphics;
			_lockables = lockables;
		}

		public void Initialize()
		{
			_body.Head.Damaged += OnDamaged;
			_body.Torso.Damaged += OnDamaged;
			_body.Legs.Damaged += OnDamaged;
		}

		public void Dispose()
		{
			_body.Head.Damaged -= OnDamaged;
			_body.Torso.Damaged -= OnDamaged;
			_body.Legs.Damaged -= OnDamaged;
		}

		public void Enter()
        {
            _kneeled = false;
            OnEntered();
            _graphics.Kneel();
		}

        public void EnterFromKneels()
		{
			_kneeled = true;
			OnEntered();
		}

		public void Process(float deltaTime)
		{
			_timestamp += deltaTime;
			if (_timestamp >= Time)
				Finished?.Invoke();
		}

		public void Exit() =>
			_lockables.ForEach(l => l.Lock());

		public bool IsKneeled() =>
			_kneeled;

		private async void OnDamaged()
		{
			if (!_isActive)
				return;

			_timestamp = 0;
			_damageCount++;
			_graphics.SetMovingSpeed(1);
			await UniTask.Delay(TimeSpan.FromSeconds(KneelingTime));
			_graphics.SetMovingSpeed(0);
			if (_damageCount >= DamageForKneeling)
				_kneeled = true;
		}

		private void OnEntered()
		{
			_timestamp = 0;
			_damageCount = 0;
			_lockables.ForEach(l => l.Unlock());
			_graphics.SetMovingSpeed(0);
			_isActive = true;
		}
	}
}