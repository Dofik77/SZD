using System;
using UnityEngine;

namespace RedRockStudio.SZD.Services.Input
{
	public interface IPointerInput
	{
		event Action<Vector2> Aimed;
		
		event Action<bool> ShootingChanged;
	}
}