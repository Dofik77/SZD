using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RedRockStudio.SZD.Enemy.Graphics
{
	public interface IRagdollBone
	{
		bool Enabled { get; }

		Rigidbody2D Rigidbody { get; }
		
		void Activate();

		void Deactivate();
	}
}