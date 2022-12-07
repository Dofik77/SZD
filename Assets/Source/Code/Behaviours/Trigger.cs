using System;
using RedRockStudio.SZD.BehavioursInterfaces;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
	public class Trigger : MonoBehaviour, ITrigger
	{
		public event Action Entered;

		public event Action Exited;

		private void OnTriggerEnter2D(Collider2D other) =>
			Entered?.Invoke();

		private void OnTriggerExit2D(Collider2D other) =>
			Exited?.Invoke();
	}
}