using System.Collections.Generic;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Animations;

namespace RedRockStudio.SZD.Character
{
	public class WeaponGraphic : ICharacterComponent, IWeaponGraphic
	{
		private static readonly int WeaponType = Animator.StringToHash("WeaponType");
		private static readonly int SwitchWeapon = Animator.StringToHash("SwitchWeapon");
		private static readonly int ReloadingMultiplier = Animator.StringToHash("ReloadingMultiplier");
		private static readonly int UsingWeapon = Animator.StringToHash("UsingWeapon");
		private static readonly int UseWeapon = Animator.StringToHash("UseWeapon");
		private static readonly int SecondHandUse = Animator.StringToHash("SecondHandUse");
		private static readonly int Reload = Animator.StringToHash("Reload");
		
		private readonly ISkeletonAnimation _skeletonAnimation;
		private readonly Animator _animator;
		private readonly ParentConstraint _sightConstraint;
		private readonly List<ConstraintSource> _sources = new();

		public WeaponGraphic(Animator animator, ISkeletonAnimation skeletonAnimation, ParentConstraint sightConstraint)
		{
			_animator = animator;
			_sightConstraint = sightConstraint;
			_skeletonAnimation = skeletonAnimation;
		}

		public void Initialize()
		{
			_sightConstraint.GetSources(_sources);
			for (int i = _sightConstraint.sourceCount - 1; i > 0; i--)
				_sightConstraint.RemoveSource(i);
		}

		public void Dispose() { }

		public void SetUp(WeaponGraphicSettings settings, float reloadingTime)
		{
			SetUpSkeleton(settings);
			_sightConstraint.SetSource(
				0, _sources.First(s => s.sourceTransform.name == settings.WeaponBone));
			_sightConstraint.SetTranslationOffset(0, settings.SightOffset);
			_animator.SetInteger(WeaponType, (int) settings.Type);
			_animator.SetTrigger(SwitchWeapon);
			_animator.SetFloat(ReloadingMultiplier, reloadingTime / settings.ReloadingAnimationLength);
			//TODO Setup pose?
		}

		public void StartUsing() =>
			_animator.SetBool(UsingWeapon, true);

		public void StopUsing() =>
			_animator.SetBool(UsingWeapon, false);

		public void Use() =>
			_animator.SetTrigger(UseWeapon);

		public void UseSecondHand() =>
			_animator.SetTrigger(SecondHandUse);

		public void ReloadWeapon() =>
			_animator.SetTrigger(Reload);

		private void SetUpSkeleton(WeaponGraphicSettings settings)
		{
			_skeletonAnimation.Skeleton.SetSkin(settings.SkinName);
			Slot slot = _skeletonAnimation.Skeleton.FindSlot(settings.WeaponBone);
			Attachment attachment = _skeletonAnimation.Skeleton.GetAttachment(settings.WeaponBone, settings.AttachmentName);
			slot.Attachment = attachment;
		}
	}
}