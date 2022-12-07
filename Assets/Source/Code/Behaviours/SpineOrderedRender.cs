using System;
using RedRockStudio.SZD.BehavioursInterfaces;
using UnityEngine;

namespace RedRockStudio.SZD.Behaviours
{
	public class SpineOrderedRender : MonoBehaviour, IOrderedRenderer
	{
		[SerializeField] private Renderer[] _renderers;

		private int _order;

		public Transform Root => transform;

		public int Order => _order;

		public void SetOrder(int value)
		{
			foreach (Renderer renderer in _renderers)
				renderer.sortingOrder = value;
			_order = value;
		}

		private void Start() =>
			_order = _renderers[0].sortingOrder;
	}
}