using Cysharp.Threading.Tasks;
using DG.Tweening;
using RedRockStudio.SZD.Enemy.Graphics;
using Spine.Unity;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
    [RequireComponent(
		typeof(Rigidbody2D), typeof(Collider2D), typeof(SkeletonUtilityBone))]
	[RequireComponent(typeof(BoneFollower))]
	public class SpineRagdollBone : MonoBehaviour, IRagdollBone
	{
		private Rigidbody2D _rigidbody;
		private SkeletonUtilityBone _utility;
		private BoneFollower _follower;
		private Tween _deactivationTween;
		
		public Rigidbody2D Rigidbody => _rigidbody;

		public bool Enabled => enabled;

        public void Activate()
		{
			if(!enabled) return;

			_deactivationTween?.Complete();
			_rigidbody.isKinematic = false;
			_follower.enabled = false;
			_utility.enabled = true;
			_utility.overrideAlpha = 1;
		}
		
		public void Deactivate()
		{
			if (!enabled) return;
			
			_rigidbody.isKinematic = true;
			_utility.enabled = false;
			_follower.enabled = true;
		}

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
			_utility = GetComponent<SkeletonUtilityBone>();
			_follower = GetComponent<BoneFollower>();
		}
	}
}