using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using RedRockStudio.SZD.Enemy.Graphics;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
	public class KickedState : IEnemyState
	{
		public event Action Finished;

		private readonly IEnemyGraphics _graphics;
		private readonly Transform _transform;
		private readonly IRagdoll _ragdoll;
		private readonly IRagdollBone _bone;

		private Tween _tween;
		private bool _interrupted;

		public KickedState(IEnemyGraphics graphics, Transform transform, IRagdoll ragdoll, IRagdollBone bone)
		{
			_graphics = graphics;
			_transform = transform;
			_ragdoll = ragdoll;
			_bone = bone;
		}

		public async void Enter()
		{
			_interrupted = false;
			_graphics.SetMovingSpeed(-2);
			_graphics.ChangeMoving(true).Forget();
			_tween = _transform.DOMove(_transform.position + Vector3.right, 1);
			await _tween;
			if(_interrupted) return;
			_graphics.SetMovingSpeed(1);
			await _graphics.ChangeMoving(false);
			if(_interrupted) return;
			Finished?.Invoke();
		}

		public async void EnterFromKneels()
		{
			_ragdoll.Activate();
			_bone.Rigidbody.AddForce(Vector2.right * 500);
			await UniTask.WaitUntil(_ragdoll.IsStopped);
			_graphics.FalledInDirection(_ragdoll.CalculateOrientation() == Orientation.Prostrate);
			await _ragdoll.Deactivate();
			Finished?.Invoke();
		}

		public void Process(float _) { }

		public void Exit()
		{
			_interrupted = true;
			_tween?.Complete();
		}
	}
}