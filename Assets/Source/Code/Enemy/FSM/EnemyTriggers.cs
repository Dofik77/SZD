using RedRockStudio.SZD.Behaviours;
using RedRockStudio.SZD.BehavioursInterfaces;

namespace RedRockStudio.SZD.Enemy
{
	public class EnemyTriggers : IEnemyComponent
	{
		private readonly IEvent _kickedEvent;
		private readonly IEnemyControl _control;
		private readonly IBody _body;
		private readonly ITrigger _trigger;

		public EnemyTriggers(IEvent kickedEvent, IEnemyControl control, IBody body, ITrigger trigger)
		{
			_kickedEvent = kickedEvent;
			_control = control;
			_body = body;
			_trigger = trigger;
		}

		public void Initialize()
		{
			_kickedEvent.Fired += _control.Kick;
			_body.Damaged += OnDamaged;
			_trigger.Entered += OnTriggerEntered;
			_trigger.Exited += OnTriggerExited;
		}

		public void Dispose()
		{
			_kickedEvent.Fired -= _control.Kick;
			_body.Damaged -= OnDamaged;
		}

		private void OnDamaged()
		{
			if (_body.Head.HaveBeenKilled(false) || _body.Torso.HaveBeenKilled(false))
				_control.Die();
			else
				_control.Damage();
		}

		private void OnTriggerEntered() =>
			_control.AttackAvaliable();

		private void OnTriggerExited() =>
			_control.AttackNotAvaliable();
	}
}