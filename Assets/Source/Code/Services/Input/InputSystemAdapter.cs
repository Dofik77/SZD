using System;
using UnityEngine.InputSystem;

namespace RedRockStudio.SZD.Services.Input
{
	public class InputSystemAdapter : IKeysInput, IService
	{
		public event Action Reloaded;

		public event Action SwitchedWeapon;

		public event Action Paused;

		public event Action<bool> MovingChanged;

		public event Action<float> Moved;

		public event Action Kicked;

		private Controls _controls;

		public void Initialize()
		{
			_controls = new Controls();
			_controls.Actions.Reload.performed += OnReloadedPerformed;
			_controls.Actions.SwitchWeapon.performed += OnSwitchPerformed;
			_controls.Actions.Move.started += OnMoveStarted;
			_controls.Actions.Move.performed += OnMovePerformed;
			_controls.Actions.Move.canceled += OnMoveCanceled;
			_controls.Actions.Kick.performed += OnKickPerformed;
			_controls.Actions.Pause.performed += OnPausePerformed;
			_controls.Enable();
		}

		public void Dispose()
		{
			_controls.Actions.Reload.performed -= OnReloadedPerformed;
			_controls.Actions.SwitchWeapon.performed -= OnSwitchPerformed;
			_controls.Actions.Move.started -= OnMoveStarted;
			_controls.Actions.Move.performed -= OnMovePerformed;
			_controls.Actions.Move.canceled -= OnMoveCanceled;
			_controls.Actions.Kick.performed -= OnKickPerformed;
			_controls = null;
		}

		private void OnReloadedPerformed(InputAction.CallbackContext context) => 
			Reloaded?.Invoke();

		private void OnSwitchPerformed(InputAction.CallbackContext context) => 
			SwitchedWeapon?.Invoke();

		private void OnMoveStarted(InputAction.CallbackContext context) =>
			MovingChanged?.Invoke(true);

		private void OnMovePerformed(InputAction.CallbackContext context) => 
			Moved?.Invoke(context.ReadValue<float>());

		private void OnMoveCanceled(InputAction.CallbackContext context) =>
			MovingChanged?.Invoke(false);

		private void OnKickPerformed(InputAction.CallbackContext context) => 
			Kicked?.Invoke();

		private void OnPausePerformed(InputAction.CallbackContext context) =>
			Paused?.Invoke();
	}
}