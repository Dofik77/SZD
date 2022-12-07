using R1noff.RData;
using UnityEngine;

namespace RedRockStudio.SZD.Extensions.RData
{
	[CreateAssetMenu(fileName = nameof(WeaponConfig), menuName = "Data/" + nameof(WeaponConfig))]
	public class WeaponConfigProvider : DataProvider<WeaponConfig> { }
}