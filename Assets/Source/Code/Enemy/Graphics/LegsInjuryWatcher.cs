using UnityEngine;

namespace RedRockStudio.SZD.Enemy.Graphics
{
	public class LegsInjuryWatcher : IEnemyComponent
	{
		private readonly Animator _animator;
		private readonly IBody _body;

		public LegsInjuryWatcher(Animator animator, IBody body)
		{
			_animator = animator;
			_body = body;
		}
		
		public void Initialize() => 
			_body.Legs.Damaged += OnLegsDamaged;

		public void Dispose() => 
			_body.Legs.Damaged -= OnLegsDamaged;

		private void OnLegsDamaged() =>
			_animator.SetInteger("LegsInjury", (int)_body.Legs.Injuries);
	}
}