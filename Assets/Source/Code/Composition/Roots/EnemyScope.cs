using R1noff.RData;
using RedRockStudio.SZD.Behaviours;
using RedRockStudio.SZD.BehavioursInterfaces;
using RedRockStudio.SZD.Enemy;
using RedRockStudio.SZD.Enemy.Graphics;
using RedRockStudio.SZD.Enemy.HandsLogic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	[RequireComponent(typeof(AnimationInstaller), typeof(RagdollInstaller), typeof(SkinInstaller))]
	[RequireComponent(typeof(BodyPartsInstaller))]
	public class EnemyScope : BaseLifetimeScope
	{
		[SerializeField] private DataProvider<EnemyConfig> _configProvider;
		[SerializeField] private SpineRagdollBone _impactBone;
		[SerializeField] private Transform _transform;
		[SerializeField] private AttackedEvent _attackedEvent;
		[SerializeField] private Trigger _hittableTrigger;

		public override void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(_transform);
			builder.Register<LegsInjuryWatcher>(Lifetime.Singleton).As<IEnemyComponent>();
			EnemyConfig config = _configProvider.Read();
			builder.RegisterInstance(config);
			builder.Register<EnemyGraphics>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<HandsAnimation>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<DamageImpact>(Lifetime.Singleton).AsImplementedInterfaces();
			RegisterStates(builder);
			builder.Register<EnemyStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
			RegisterTriggers(builder);
			builder.RegisterEntryPoint<EnemyEntryPoint>();
		}

		private void RegisterStates(IContainerBuilder builder)
		{
			builder.Register<AttackingState>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<CrawlingBackState>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<FallingState>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<IdleState>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<KickedState>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter<IRagdollBone>(_impactBone);
			builder.Register<MovingState>(Lifetime.Singleton).AsImplementedInterfaces().WithParameter(_transform);
			builder.Register<RisingState>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<DeathState>(Lifetime.Singleton).AsImplementedInterfaces();
		}

		private void RegisterTriggers(IContainerBuilder builder) =>
			builder.Register<EnemyTriggers>(Lifetime.Singleton)
			       .AsImplementedInterfaces()
			       .WithParameter<IEvent>(_attackedEvent)
			       .WithParameter<ITrigger>(_hittableTrigger);
	}
}