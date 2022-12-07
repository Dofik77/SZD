using System;
using UnityEngine;

namespace RedRockStudio.SZD.Character
{
	[Serializable]
	public class WeaponGraphicSettings
	{
		[SerializeField] private AnimationClip _reloadingAnimation;

		public float ReloadingAnimationLength => _reloadingAnimation.length;

		[field: Header("Skeleton")]
		[field: SerializeField] public string SkinName { get; private set; }

		[field: SerializeField] public string WeaponBone { get; private set; }

		[field: SerializeField] public string AttachmentName { get; private set; }

		[field: Header("Animation")]
		[field: SerializeField] public WeaponType Type { get; private set; }

		[field: SerializeField] public Vector3 SightOffset;
	}
}