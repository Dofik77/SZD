using UnityEngine;

namespace RedRockStudio.SZD.Enemy.HandsLogic
{
	public class HandsAnimation : ILockable, IEnemyComponent
	{
		private static readonly int DefHead = Animator.StringToHash("DefHead");
		private static readonly int DefTorso = Animator.StringToHash("DefTorso");
		
		private readonly IBody _body;
		private readonly Animator _animator;
		
		private bool _locked = true;

		public HandsAnimation(IBody body, Animator animator)
		{
			_body = body;
			_animator = animator;
		}

		public void Initialize()
		{
			_body.Head.Damaged += OnHeadDamaged;
			_body.Torso.Damaged += OnTorsoDamaged;
		}

		public void Dispose()
		{
			_body.Head.Damaged -= OnHeadDamaged;
			_body.Torso.Damaged -= OnTorsoDamaged;
		}

		public void Lock()
		{
			_animator.SetBool(DefHead, false);
			_animator.SetBool(DefTorso, false);
			_locked = true;
		}

		public void Unlock() => 
			_locked = false;

		private void OnHeadDamaged()
		{
			if (_locked)
				return;
			_animator.SetBool(DefHead, true);
			_animator.SetBool(DefTorso, false);
		}

		private void OnTorsoDamaged()
		{
			if (_locked)
				return;
			_animator.SetBool(DefHead, false);
			_animator.SetBool(DefTorso, true);
		}
	}
}