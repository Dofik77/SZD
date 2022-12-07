using Spine.Unity;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
	public class SkeletonReload : MonoBehaviour
	{
		private void Start() =>
			GetComponent<SkeletonRenderer>().Initialize(true);
	}
}