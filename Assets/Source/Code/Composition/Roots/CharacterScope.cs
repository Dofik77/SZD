using MoreMountains.Feedbacks;
using R1noff.RData;
using RedRockStudio.SZD.Character;
using RedRockStudio.SZD.Composition.Factories;
using UnityEngine;
using UnityEngine.Animations;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	[RequireComponent(typeof(AnimationInstaller))]
	[RequireComponent(typeof(InputInstaller),typeof(TestWeaponDataInstaller))]//TODO move
	public class CharacterScope : BaseLifetimeScope
	{
		[SerializeField] private Transform _aimTransform;
		[SerializeField] private Camera _camera;
		[SerializeField] private MMF_Player _feedbacks;
		[SerializeField] private DataProvider<MovementConfig> _configProvider;
		[SerializeField] private Rigidbody2D _rigidbody;
		[SerializeField] private ParentConstraint _sightConstraint;
		[SerializeField] private GameObject _fxPrefab;
		[SerializeField] private LayerMask _shootingLayerMask;

		public override void Install(IContainerBuilder builder)
		{
			builder.RegisterInstance(_configProvider.Read());
			builder.RegisterInstance(_rigidbody);
			builder.RegisterInstance(_sightConstraint).As<IConstraint>();
			builder.RegisterInstance(_feedbacks).As<MMFeedbacks>();
			builder.Register<WeaponGraphic>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.Register<MovementFsm>(Lifetime.Singleton).AsImplementedInterfaces();
			RegisterAiming(builder);
			RegisterShooting(builder);
			builder.RegisterFactoryFunc<WeaponFactory, WeaponData, IWeapon>();
			builder.Register<Equipment>(Lifetime.Singleton).AsImplementedInterfaces();
			builder.RegisterEntryPoint<CharacterEntryPoint>();
		}

		private void RegisterAiming(IContainerBuilder builder) =>
			builder.Register<Aiming>(Lifetime.Singleton)
			       .WithParameter(_aimTransform)
			       .WithParameter(_camera)
			       .AsImplementedInterfaces();

		private void RegisterShooting(IContainerBuilder builder) =>
			builder.Register<Shooting>(Lifetime.Singleton)
			       .As<IShooting>()
			       .WithParameter(_fxPrefab)
			       .WithParameter(_shootingLayerMask);
	}
}