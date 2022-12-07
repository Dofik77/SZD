using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using RedRockStudio.SZD.Common;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy.Graphics
{
	//TODO Better approach
	public class EnemyGraphics : IEnemyGraphics, IEnemyComponent
	{
		private const float RisingDuration = 3;
		private const float AttackDuration = 0.833f;
		private const float KneeAttackDuration = 1.167f;

		private static readonly int Running = Animator.StringToHash("Running");
		private static readonly int FallTrigger = Animator.StringToHash("Fall");
		private static readonly int AttackTrigger = Animator.StringToHash("Attack");
		private static readonly int BiteTrigger = Animator.StringToHash("Bite");
		private static readonly int RiseTrigger = Animator.StringToHash("Rise");
		private static readonly int KneelTrigger = Animator.StringToHash("Kneel");
		private static readonly int FallDirection = Animator.StringToHash("FallDirection");
		private static readonly int MovingSpeed = Animator.StringToHash("MovingSpeed");
		private static readonly int Moving = Animator.StringToHash("Moving");

		private readonly Animator _animator;
		private readonly ISkin _skin;

		public EnemyGraphics(Animator animator, ISkin skin)
		{
			_animator = animator;
			_skin = skin;
		}

		public async void Initialize()
		{
			await UniTask.DelayFrame(2);
			_skin.Apply();
		}

		public void Dispose() { }

		public void Fall()
		{
			_animator.SetInteger(FallDirection, 0);
			_animator.SetTrigger(FallTrigger);
		}

		public UniTask Attack(bool kneeled)
		{
			_animator.SetTrigger(AttackTrigger);
			return UniTask.Delay(TimeSpan.FromSeconds(kneeled ? KneeAttackDuration : AttackDuration));
		}

		public UniTask Bite(bool kneeled)
		{
			_animator.SetTrigger(BiteTrigger);
			return UniTask.Delay(TimeSpan.FromSeconds(kneeled ? KneeAttackDuration : AttackDuration));
		}

		public UniTask Rise()
		{
			_animator.SetTrigger(RiseTrigger);
			return UniTask.Delay(TimeSpan.FromSeconds(RisingDuration));
		}

		public void Kneel()  =>
			_animator.SetTrigger(KneelTrigger);

		public void FalledInDirection(bool forward) => 
			_animator.SetInteger(FallDirection, forward ? -1 : 1);

		public void SetMovingSpeed(float value) =>
			_animator.SetFloat(MovingSpeed, value);

		public UniTask ChangeMoving(bool value)
		{
			_animator.SetBool(Moving, value);
			AnimatorClipInfo[] info = _animator.GetCurrentAnimatorClipInfo(0);
			return info.Length > 0 ? UniTask.Delay(TimeSpan.FromSeconds(info[0].clip.length)) : UniTask.CompletedTask;
		}

		public void ChangeRunning(bool value) => 
			_animator.SetBool(Running, value);
	}
}