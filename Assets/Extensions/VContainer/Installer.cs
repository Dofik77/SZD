using UnityEngine;

namespace VContainer.Unity
{
	public abstract class Installer : MonoBehaviour, IInstaller
	{
		public abstract void Install(IContainerBuilder builder);
	}
}