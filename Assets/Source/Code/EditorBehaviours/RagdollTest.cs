using System.Linq;
using RedRockStudio.SZD.Behaviours;
using RedRockStudio.SZD.Enemy;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

public class RagdollTest : MonoBehaviour
{
	[SerializeField] private SpineRagdollBone[] _bones;

	[Button]
	private void Enable()
	{
		//var rd = new SpineRagdoll(GetComponent<ISkeletonAnimation>(), _bones, transform, _bones.First());
		//rd.Activate();
	}
	
	[Button]
	private void Disable()
	{
		//var rd = new SpineRagdoll(GetComponent<ISkeletonAnimation>(), _bones, transform, _bones.First());
		//rd.Deactivate();
	}
	
	[Button]
	private void AddForce(Rigidbody2D rigidbody, Vector2 direction)
	{
		Enable();
		rigidbody.AddForce(direction);
	}

	[Button]
	private void AddForce(Rigidbody2D rigidbody, Vector2 direction, Vector3 position)
	{
		Enable();
		rigidbody.AddForceAtPosition(direction, position);
	}

	[Button]
	private void CollectBones(GameObject target) => 
		_bones = target.GetComponentsInChildren<SpineRagdollBone>();
}
