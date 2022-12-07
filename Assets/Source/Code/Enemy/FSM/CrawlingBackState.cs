using RedRockStudio.SZD.Enemy.Graphics;
using System;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
	public class CrawlingBackState : IEnemyState
	{
		private const float IdleTime = 1.5f;
		private const float CrawlingTime = 2.5f;

		public event Action Finished
		{
			add => throw new NotSupportedException();
			remove { }
		}

		private readonly IEnemyGraphics _graphics;
		private readonly Transform _transform;
		private readonly float _speed;

		private bool _isCrawling;
		private float _timestamp;

		public CrawlingBackState(IEnemyGraphics graphics, Transform transform, EnemyConfig config)
        {
			_graphics = graphics;
			_transform = transform;
			_speed = config.CrawlingBackSpeed;
        }

		public void Enter() =>
			throw new NotSupportedException();

		public void EnterFromKneels()
        {
			_isCrawling = false;
        }

		public void Process(float deltaTime)
        {
			_timestamp += deltaTime;
			if (_isCrawling && _timestamp >= CrawlingTime ||
				!_isCrawling && _timestamp >= IdleTime)
            {
				_timestamp = 0f;
				_isCrawling = !_isCrawling;
				_graphics.ChangeMoving(_isCrawling);
				return;
            }

			if(_isCrawling)
				_transform.Translate(Vector3.right * _speed * deltaTime);
		}

		public void Exit() { }
	}
}