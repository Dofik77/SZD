using System.Collections.Generic;
using Extensions.VContainer;
using RedRockStudio.SZD.Services;

namespace RedRockStudio.SZD.Composition
{
	public class ServicesEntryPoint : BaseEntryPoint<IService>
	{
		public ServicesEntryPoint(IEnumerable<IService> components) : base(components) { }

		protected override void Start(IService value) =>
			value.Initialize();

		protected override void Dispose(IService value) =>
			value.Dispose();

	}
}