using System;

namespace RedRockStudio.SZD.Services.Input
{
	public interface IKeysInput
	{
		event Action Reloaded;

	    event Action SwitchedWeapon;

	    event Action Paused;

	    event Action<bool> MovingChanged; 

	    event Action<float> Moved;

	    event Action Kicked;
    }
}
