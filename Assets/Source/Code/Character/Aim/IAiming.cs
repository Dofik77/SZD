using System;
using UnityEngine;

namespace RedRockStudio.SZD.Character
{
	public interface IAiming
	{
		Vector2 Direction { get; }
		
		Vector2 MuzzlePosition { get; }
		
		void AnimateSwitching(Action switchedCallback, Action endedCallback);
	}
}