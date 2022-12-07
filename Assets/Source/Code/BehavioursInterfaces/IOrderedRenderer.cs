using UnityEngine;

namespace RedRockStudio.SZD.BehavioursInterfaces
{
	public interface IOrderedRenderer
	{
		int Order { get; }

		Transform Root { get; }

		void SetOrder(int value);
	}
}