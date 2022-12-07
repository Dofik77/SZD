using System;
using RedRockStudio.SZD.Common.DamageData;

namespace RedRockStudio.SZD.Enemy
{
	public interface IDamageable
	{
		event Action<DamageData> Damaged; 

		void ProcessDamage(DamageData data);
	}
}