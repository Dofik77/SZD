using RedRockStudio.SZD.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
	public class ImpactTest : MonoBehaviour
	{
		[SerializeField] private SpineRagdollBone _bone;

		[Button]
		private void Apply(float angle)
		{
			_bone.Activate();
			transform.Rotate(Vector3.forward, angle);
			_bone.Deactivate();
		}
	}
}