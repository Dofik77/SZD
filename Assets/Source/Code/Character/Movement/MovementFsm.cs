using System;
using Cysharp.Threading.Tasks;
using RedRockStudio.SZD.Services.Input;
using Stateless;
using UnityEngine;
using IDisposable = System.IDisposable;

namespace RedRockStudio.SZD.Character
{
	public class MovementFsm : IMovement, ICharacterComponent
	{
		private enum State
		{
			Idle,
			Moving,
			Kicking
		}
		
		private enum Trigger
		{
			MoveInput,
			KickInput,
			StopInput,
			StopKick
		}

		private readonly MovementConfig _config;
		private readonly Animator _animator;
		private readonly Rigidbody2D _rigidbody2D;
		private readonly IKeysInput _keysInput;
		
		private StateMachine<State, Trigger> _fsm;
		private static readonly int MovingParameter = Animator.StringToHash("Moving");
		private static readonly int KickParameter = Animator.StringToHash("Kick");

		public bool IsKicking { get; private set; }
		
		public MovementFsm(MovementConfig config, Animator animator, Rigidbody2D rigidbody2D, IKeysInput keysInput)
		{
			_config = config;
			_animator = animator;
			_rigidbody2D = rigidbody2D;
			_keysInput = keysInput;
		}

		public void Initialize()
		{
			_fsm = new StateMachine<State, Trigger>(State.Idle);
			ConfigureIdleState();
			ConfigureMovingState();
			ConfigureKickingState();
			_keysInput.Moved += Move;
			_keysInput.Kicked += Kick;
			_keysInput.MovingChanged += OnMovingChanged;
		}
		
		public void Dispose()
		{
			_keysInput.Moved -= Move;
			_keysInput.Kicked -= Kick;
			_keysInput.MovingChanged -= OnMovingChanged;
		}

		private void ConfigureKickingState() =>
			_fsm.Configure(State.Kicking)
			    .Permit(Trigger.StopKick, State.Idle)
			    .Ignore(Trigger.MoveInput)
			    .Ignore(Trigger.StopInput)
			    .Ignore(Trigger.KickInput)
			    .OnEntryAsync(
				    async () =>
				    {
					    IsKicking = true;
					    _animator.SetTrigger(KickParameter);
					    await UniTask.Delay(TimeSpan.FromSeconds(_config.KickingTime));
					    IsKicking = false;
					    _fsm.Fire(Trigger.StopKick);
				    });

		private void ConfigureMovingState() =>
			_fsm.Configure(State.Moving)
			    .Permit(Trigger.StopInput, State.Idle)
			    .Permit(Trigger.KickInput, State.Kicking)
			    .Ignore(Trigger.MoveInput)
			    .Ignore(Trigger.StopKick)
			    .OnExit(() =>
				    {
					    _rigidbody2D.velocity = Vector2.zero;
					    _animator.SetFloat(MovingParameter, 0);
				    });

		private void ConfigureIdleState() => 
			_fsm.Configure(State.Idle)
			    .Permit(Trigger.MoveInput, State.Moving)
			    .Permit(Trigger.KickInput, State.Kicking)
			    .Ignore(Trigger.StopInput)
			    .Ignore(Trigger.StopKick);

		private void OnMovingChanged(bool newState) => 
			_fsm.Fire(newState ? Trigger.MoveInput : Trigger.StopInput);

		private void Move(float direction)
		{
			if (_fsm.IsInState(State.Moving))
			{
				_rigidbody2D.velocity = Vector2.right * _config.Speed * direction;
				_animator.SetFloat(MovingParameter, direction);
			}
		}

		private void Kick() => 
			_fsm.FireAsync(Trigger.KickInput);
	}
}