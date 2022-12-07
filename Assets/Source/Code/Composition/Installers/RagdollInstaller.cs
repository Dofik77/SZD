using System.Collections.Generic;
using System.Linq;
using RedRockStudio.SZD.Behaviours;
using RedRockStudio.SZD.Enemy;
using RedRockStudio.SZD.Enemy.Graphics;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	public sealed class RagdollInstaller : Installer
	{
		[SerializeField] private SpineRagdollBone[] _bones;

		public override void Install(IContainerBuilder builder) =>
			builder.Register<SpineRagdoll>(Lifetime.Singleton)
			       .As<IRagdoll>()
			       .WithParameter<IEnumerable<IRagdollBone>>(_bones)
			       .WithParameter(transform)
			       .WithParameter<IRagdollBone>(_bones.First());

		[Button]
		private void CollectBones() => 
			_bones = transform.GetComponentsInChildren<SpineRagdollBone>();
	}
}