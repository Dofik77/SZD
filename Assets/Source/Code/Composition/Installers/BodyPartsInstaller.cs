using System;
using RedRockStudio.SZD.Behaviours;
using RedRockStudio.SZD.Enemy;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using VContainer;
using VContainer.Unity;

namespace RedRockStudio.SZD.Composition
{
	public class BodyPartsInstaller : Installer
	{
		public interface IBodyPartCreator
		{
			IBodyPart Create();
		}
		
		[Serializable]
		[MovedFrom(
			true,
			"RedRockStudio.SZD.Composition",
			"Assembly-CSharp",
			"BodyInstaller/BodyPartCreator")]
		private class BodyPartCreator : IBodyPartCreator
		{
			[SerializeField] private Rigidbody2D _rigidbody;
			[SerializeField] private Damageable _damageable;
			[SerializeField] private int _maxHealth;
			[SerializeField] private bool _debug;
			[SerializeField] [ShowIf(nameof(_debug))] private Renderer[] _renderers;
			[SerializeField] [ShowIf(nameof(_debug))] private string _name;

			public IBodyPart Create()
			{
				var bodyPart = new BodyPart(_rigidbody, _damageable, _maxHealth);
#if DEBUG
				if(_debug)
					return new DebugBodyPart(bodyPart, _renderers, _damageable, _maxHealth, _name);
				return bodyPart;
#else
				return bodyPart;
#endif
			}
		}

		[Serializable]
		[MovedFrom(
			true,
			"RedRockStudio.SZD.Composition",
			"Assembly-CSharp",
			"BodyInstaller/DetachableBodyPartCreator")]
		private class DetachableBodyPartCreator : IBodyPartCreator
		{
			[SerializeField] private Rigidbody2D _rigidbody;
			[SerializeField] private Damageable _damageable;
			[SerializeField] private DetachableSpineBone _bone;
			[SerializeField] private int _maxHealth;
			[SerializeField] private bool _debug;
			[SerializeField] [ShowIf(nameof(_debug))] private Renderer[] _renderers;
			[SerializeField] [ShowIf(nameof(_debug))] private string _name;

			public IBodyPart Create()
			{
				var bodyPart = new DetachableBodyPart(_bone, _rigidbody, _damageable, _maxHealth);
#if DEBUG
				if(_debug)
					return new DebugBodyPart(bodyPart, _renderers, _damageable, _maxHealth, _name);
				return bodyPart;
#else
				return bodyPart;
#endif
			}
		}

		[Serializable]
		[MovedFrom(
			true,
			"RedRockStudio.SZD.Composition",
			"Assembly-CSharp",
			"BodyInstaller/DualBodyPartCreator")]
		private class DualBodyPartCreator : IBodyPartCreator
		{
			[SerializeReference] private IBodyPartCreator _first;
			[SerializeReference] private IBodyPartCreator _second;

			public IBodyPart Create() =>
				new DualBodyPart(_first.Create(), _second.Create());
		}

		[SerializeReference] private IBodyPartCreator _headCreator;
		[SerializeReference] private IBodyPartCreator _torsoCreator;
		[SerializeReference] private IBodyPartCreator _armsCreator;
		[SerializeReference] private IBodyPartCreator _legsCreator;

		public override void Install(IContainerBuilder builder)
		{
			IBodyPart head = _headCreator.Create();
			IBodyPart torso = _torsoCreator.Create();
			IBodyPart arms = _armsCreator.Create();
			IBodyPart legs = _legsCreator.Create();
			IBody body = new Body(head, torso, arms, legs);
			builder.RegisterInstance(body).As<IBody>().As<IEnemyComponent>();
		}
	}
}