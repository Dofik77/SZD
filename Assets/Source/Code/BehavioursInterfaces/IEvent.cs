using System;

namespace RedRockStudio.SZD.Behaviours
{
	public interface IEvent
	{
		event Action Fired;
	}
}