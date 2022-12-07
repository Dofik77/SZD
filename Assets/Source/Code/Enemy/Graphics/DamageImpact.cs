using DG.Tweening;
using RedRockStudio.SZD.Enemy.HandsLogic;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy.Graphics
{
    public class DamageImpact : IEnemyComponent, ILockable
	{
		private static readonly int ImpactBack = Animator.StringToHash("ImpactBack");
		private static readonly int ImpactForward = Animator.StringToHash("ImpactForward");
		
		private readonly IBody _body;
		private readonly Animator _animator;

		private bool _locked;

		public DamageImpact(IBody body, Animator animator)
		{
			_body = body;
			_animator = animator;
		}

		public void Initialize()
		{
			_body.Head.Damaged += ApplyUpperImpact;
			_body.Arms.Damaged += ApplyUpperImpact;
			_body.Torso.Damaged += ApplyUpperImpact;
			_body.Legs.Damaged += ApplyLowerImpact;
		}

		public void Dispose()
		{
			_body.Head.Damaged -= ApplyUpperImpact;
			_body.Arms.Damaged -= ApplyUpperImpact;
			_body.Torso.Damaged -= ApplyUpperImpact;
			_body.Legs.Damaged -= ApplyLowerImpact;
		}

		public void Lock() =>
			_locked = true;

		public void Unlock() =>
			_locked = false;

		private void ApplyUpperImpact()
		{
			if(!_locked)
				_animator.SetTrigger(ImpactBack);
		}

		private void ApplyLowerImpact()
		{
			if(!_locked)
				_animator.SetTrigger(ImpactForward);
		}
	}
}