using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using MyBox;
using RedRockStudio.SZD.Enemy.Graphics;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy
{
	public class SpineRagdoll : IRagdoll
	{
		private const int DeactivationDuration = 1;
		
		private static readonly Vector3 ProstrateBodyPosition = new(0.05f, 0.1f);
		private static readonly Vector3 SupineBodyPosition = new(-0.66f, 0.08f);
		
		private readonly ISkeletonAnimation _skeletonAnimation;
		private readonly IEnumerable<IRagdollBone> _bones;
		private readonly Transform _transform;
		private readonly IRagdollBone _rootBone;

		public bool IsActive { get; private set; }

		public SpineRagdoll(
			ISkeletonAnimation skeletonAnimation, IEnumerable<IRagdollBone> bones, Transform transform,
			IRagdollBone rootBone)
		{
			_skeletonAnimation = skeletonAnimation;
			_bones = bones;
			_transform = transform;
			_rootBone = rootBone;
		}

		public void Activate()
		{
			IsActive = true;
			SetIK(false);
			foreach (IRagdollBone bone in _bones)
				bone.Activate();
		}

		public bool IsStopped() =>
			_bones.All(b => !b.Enabled || b.Rigidbody.velocity.magnitude <= 0.01f);

		public Orientation CalculateOrientation() =>
			_rootBone.Rigidbody.rotation switch
			{
				>= -30 => Orientation.Prostrate,
				<= -150 => Orientation.Supine,
				_ => Orientation.Straight
			};

		public async UniTask Deactivate()
		{
			SetSkeletonPosition();
			await SetIK(true);
			
			foreach (IRagdollBone bone in _bones)
				bone.Deactivate();
			IsActive = false;
		}

		private void SetSkeletonPosition()
		{
			Vector3 current = _rootBone.Rigidbody.transform.position;
			Vector3 targetLocal = CalculateOrientation() == Orientation.Prostrate ? 
				ProstrateBodyPosition : SupineBodyPosition;
			Vector3 target = _transform.TransformPoint(targetLocal);
			Vector3 offset = current - target;
			_transform.OffsetXY(offset.x, offset.y);
			foreach (IRagdollBone ragdollBone in _bones)
				ragdollBone.Rigidbody.transform.Translate(-offset.x, -offset.y, 0, Space.World);
		}

		private UniTask SetIK(bool value) =>
			UniTask.WhenAll(
				_skeletonAnimation.Skeleton.IkConstraints.Items.Select(
					c => SetIKConstraintMix(c, value ? 1 : 0, value ? DeactivationDuration : 0)));

		private UniTask SetIKConstraintMix(IkConstraint constraint, float value, float duration) =>
			DOTween.To(() => constraint.Mix, x => constraint.Mix = x, value, duration).ToUniTask();
	}
}