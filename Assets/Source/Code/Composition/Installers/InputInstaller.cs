using RedRockStudio.SZD.Services.Input;
using RedRockStudio.SZD.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	public sealed class InputInstaller : Installer
	{
		[SerializeField] private TouchInput _touch;
		
		public override void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(_touch).As<IPointerInput>();
			builder.Register<InputSystemAdapter>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<PauseHandler>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.RegisterEntryPoint<ServicesEntryPoint>();
		}
	}
}