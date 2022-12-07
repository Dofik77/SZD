using System.Collections;
using System.Linq;
using RedRockStudio.SZD.BehavioursInterfaces;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
	[RequireComponent(typeof(Rigidbody2D))]
	public class DetachableSpineBone : MonoBehaviour, IDetachableBone
	{
		[SerializeField] private GameObject _renderObject;
		[SerializeField] private Behaviour[] _disableComponents;

		private Rigidbody2D _rigidbody;

		[Button]
		public void Detach()
		{
			//TODO ik
			//_skeletonUtilityBone = GetComponent<SkeletonUtilityBone>();
			//IkConstraint ik = _skeletonUtilityBone
			//                  .hierarchy.Skeleton.IkConstraints
			//                  .FirstOrDefault(c => c.Bones.Any(b => b.Data.Name == _skeletonUtilityBone.boneName));

			//ik?.Bones.RemoveAll(b => b.Data.Name == _skeletonUtilityBone.boneName);
			//_skeletonUtilityBone
			//	.hierarchy.Skeleton.UpdateCacheList
			//	.RemoveAll(b => (b as Bone)?.Data.Name == _skeletonUtilityBone.boneName);
			foreach (var component in _disableComponents)
				component.enabled = false;

			GameObject render = Instantiate(_renderObject, transform, true);
			_renderObject.SetActive(false);
			foreach (var part in render.GetComponentsInChildren<SkeletonPartsRenderer>())
			{
				part.MeshFilter.mesh = Instantiate(part.MeshFilter.sharedMesh);
				Destroy(part);
			}

			transform.SetParent(null);
			_rigidbody.isKinematic = false;
		}

		private void Awake()
		{
			_rigidbody = GetComponent<Rigidbody2D>();
		}
	}
}