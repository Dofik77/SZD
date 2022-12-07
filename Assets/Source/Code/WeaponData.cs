using System;
using R1noff.RData;
using RedRockStudio.SZD.Character;
using Sirenix.OdinInspector;
using TypeReferences;
using UnityEngine;

namespace RedRockStudio.SZD
{
	[Serializable]
	public class WeaponData
	{
		[field: SerializeField] public DataProvider<WeaponConfig> WeaponConfigProvider { get; private set; }

		[field: Title("SETTINGS")]
		[field: SerializeField] public float RecoilAngle { get; private set; }


		[field: Title("GRAPHIC SETTINGS")]
		[field: SerializeField] public WeaponGraphicSettings GraphicSettings { get; private set; }
		
		[field: Title("MENU SETTING")]
		[field: SerializeField] public Sprite Icon { get; private set; }

		[field: SerializeField] public string Category { get; private set; }

		[field: Title("LOGIC")]
		[field: SerializeField] [field: Inherits(typeof(IWeaponUsingState))] public TypeReference UsingType;
		[field: SerializeField] [field: Inherits(typeof(IReloadingState))] public TypeReference ReloadingType;
	}

	[Serializable]
	public class WeaponConfig
	{
		public string Name;

		public int Cost;

		public int Damage;
		
		public int AmmoSize;
		
		public float FireRatePerMinute;

		public float ReloadingTime;

		public float Income;

		public int ArmorDamage;

		public SpriteMask DamageDecal;
	}

	public enum WeaponType
	{
		Pistol = 0,
		TwoHandedPistols = 1,
		Rifle = 2,
		Launcher = 3,
		Hand = 4
	}
	
	public enum WeaponCategory
	{
		Pistols,
		Rifles,
		Shotguns,
		Other
	}
}