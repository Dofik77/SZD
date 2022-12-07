using System;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
	public class AttackedEvent : MonoBehaviour, IEvent
	{
		public event Action Fired;
		
		public void Kick() => 
			Fired?.Invoke();
	}
}