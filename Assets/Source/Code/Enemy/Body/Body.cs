using System;

namespace RedRockStudio.SZD.Enemy
{
	public class Body : IBody, IEnemyComponent
	{
		public event Action Damaged;
		
		public IBodyPart Head { get; }

		public IBodyPart Torso { get; }

		public IBodyPart Arms { get; }

		public IBodyPart Legs { get; }

		public Body(IBodyPart head, IBodyPart torso, IBodyPart arms, IBodyPart legs)
		{
			Head = head;
			Torso = torso;
			Arms = arms;
			Legs = legs;
		}
		
		public void Initialize()
		{
			Head.Initialize();
			Torso.Initialize();
			Arms.Initialize();
			Legs.Initialize();
			Head.Damaged += OnDamaged;
			Torso.Damaged += OnDamaged;
			Arms.Damaged += OnDamaged;
			Legs.Damaged += OnDamaged;
		}

		public void Dispose()
		{
			Head.Damaged -= OnDamaged;
			Torso.Damaged -= OnDamaged;
			Arms.Damaged -= OnDamaged;
			Legs.Damaged -= OnDamaged;
			Head.Dispose();
			Torso.Dispose();
			Arms.Dispose();
			Legs.Dispose();
		}
		
		private void OnDamaged() => 
			Damaged?.Invoke();
	}
}