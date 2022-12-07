using System;
using UnityEngine;

namespace RedRockStudio.SZD
{
	[Serializable]
	public class EnemyConfig
	{
		[field: SerializeField] public bool IsRunning { get; set; }

		[field: SerializeField] public float RunningSpeed { get; private set; }
		
		[field: SerializeField] public float WalkingSpeed { get; private set; }
		
		[field: SerializeField] public float LimpingSpeed { get; private set; }
		
		[field: SerializeField] public float CrawlingSpeed { get; private set; }
		
		[field: SerializeField] public float CrawlingBackSpeed { get; private set; }
	}
}