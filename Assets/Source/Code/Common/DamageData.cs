using System;
using UnityEngine;

namespace RedRockStudio.SZD.Common.DamageData
{
	[Serializable]
	public struct DamageData
	{
		[field: SerializeField] public int Value { get; private set; }

		[field: SerializeField] public float ArmorImpact { get; private set; }

		[field: SerializeField] public Vector2 Position { get; private set; }

		[field: SerializeField] public Vector2 Direction { get; private set; }
		
		public DamageData(int value, float armorImpact, Vector2 position, Vector2 direction)
		{
			Value = value;
			ArmorImpact = armorImpact;
			Position = position;
			Direction = direction;
		}

		public override string ToString() => $"{Value} damage + {ArmorImpact} on armor at {Position} with {Direction}";
	}
}