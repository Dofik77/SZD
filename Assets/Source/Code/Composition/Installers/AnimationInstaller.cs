using System;
using Spine.Unity;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	[Serializable]
	public sealed class AnimationInstaller : Installer
	{
		[SerializeField] private Animator _animator;
		[SerializeField] private SkeletonMecanim _mecanim;
		
		public override void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(_animator);
			builder.RegisterInstance(_mecanim).As<ISkeletonAnimation>().AsSelf();
		}
	}
}