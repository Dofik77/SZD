using System;

namespace RedRockStudio.SZD.BehavioursInterfaces
{
	public interface ITrigger
	{
		event Action Entered;

		event Action Exited;
	}
}