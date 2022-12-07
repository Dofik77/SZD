using System;
using RedRockStudio.SZD.Common.DamageData;
using RedRockStudio.SZD.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
	public class Damageable : MonoBehaviour, IDamageable
	{
		public event Action<DamageData> Damaged;

		[Button]
		public void ProcessDamage(DamageData data) => 
			Damaged?.Invoke(data);
	}
}