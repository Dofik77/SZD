using System;
using DG.Tweening;
using RedRockStudio.SZD.Services.Input;
using UnityEngine;

namespace RedRockStudio.SZD.Character
{
	public class Aiming : IAiming, ICharacterComponent
	{
		private const float SwitchingAngle = -90;
		
		private readonly IPointerInput _pointerInput;
		private readonly Transform _transform;
		private readonly Camera _camera;

		private bool _blocked;
		private Sequence _sequence;

		public Vector2 Direction => _transform.right;

		public Vector2 MuzzlePosition => _transform.position;

		public Aiming(Transform transform, IPointerInput pointerInput, Camera camera)
		{
			_pointerInput = pointerInput;
			_transform = transform;
			_camera = camera;
		}

		public void Initialize() => 
			_pointerInput.Aimed += Aim;

		public void Dispose() => 
			_pointerInput.Aimed -= Aim;

		public void AnimateSwitching(Action switchedCallback, Action endedCallback)
		{
			_blocked = true;
			_sequence?.Kill();
			float deltaAngle = Mathf.DeltaAngle(SwitchingAngle, _transform.rotation.eulerAngles.z);
			var speedPerSec = 180f;
			float time = deltaAngle / speedPerSec;
			_sequence = DOTween.Sequence()
			                   .Append(_transform.DORotate(Vector3.forward * SwitchingAngle, time))
			                   .AppendCallback(switchedCallback.Invoke)
			                   .Append(_transform.DORotate(Vector3.zero, 90 / speedPerSec))
			                   .AppendCallback(() => _blocked = false)
			                   .AppendCallback(endedCallback.Invoke);
		}

		private void Aim(Vector2 screenPosition)
		{
			if(_blocked) return;
			Vector3 worldPosition = _camera.ScreenToWorldPoint(
				(Vector3)screenPosition - Vector3.forward * _camera.transform.position.z);
			_transform.right = worldPosition - _transform.position;
		}
	}
}