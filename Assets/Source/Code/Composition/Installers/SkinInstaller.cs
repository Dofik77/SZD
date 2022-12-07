using System.Collections.Generic;
using R1noff.RData;
using RedRockStudio.SZD.Common;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	public class SkinInstaller : Installer
	{
		[SerializeField] private DataProvider<SkinAttachmentsData> _dataProvider;

		private ISkin _skin;
		
		public override void Install(IContainerBuilder builder)
		{
			SkinAttachmentsData data = _dataProvider.Read();
			builder.Register<SpinePerSlotSkin>(Lifetime.Singleton)
			       .As<ISkin>()
			       .WithParameter(data.GetEnabledAttachments())
			       .WithParameter<IEnumerable<string>>(data.DisabledSlots);
#if UNITY_EDITOR
			builder.RegisterBuildCallback(c => _skin = c.Resolve<ISkin>());
#endif
		}

		[Button]
		private void Apply() =>
			_skin.Apply();
	}
}