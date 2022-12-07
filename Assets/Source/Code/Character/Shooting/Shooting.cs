using MoreMountains.Feedbacks;
using MyBox;
using RedRockStudio.SZD.BehavioursInterfaces;
using RedRockStudio.SZD.Common.DamageData;
using RedRockStudio.SZD.Enemy;
using UnityEngine;

namespace RedRockStudio.SZD.Character
{
	public class Shooting : IShooting
	{
		private const float Distance = 30;
		
		private readonly IAiming _aiming;
		private readonly MMFeedbacks _feedbacks;
		private readonly GameObject _fx;
		private readonly LayerMask _layerMask;

		public Shooting(IAiming aiming, MMFeedbacks feedbacks, GameObject fx, LayerMask layerMask)//TODO fx pool
		{
			_aiming = aiming;
			_feedbacks = feedbacks;
			_fx = fx;
			_layerMask = layerMask;
		}
		
		public void Shoot(WeaponConfig config)
		{
			_feedbacks.ResetFeedbacks();
			RaycastHit2D hit = Physics2D.Raycast(_aiming.MuzzlePosition, _aiming.Direction, Distance, _layerMask);
			if (hit && hit.transform.TryGetComponent(out IDamageable damageable))
			{
				var data = new DamageData(
					config.Damage, config.ArmorDamage, hit.point, _aiming.Direction);
				damageable.ProcessDamage(data);
				TrySpawnDecal(hit, config.DamageDecal);
				Object.Instantiate(_fx, hit.point.ToVector3().SetZ(1), Quaternion.identity);
			}
			_feedbacks.PlayFeedbacks();
		}

		private static void TrySpawnDecal(RaycastHit2D hit, SpriteMask damageDecal)
		{
			if (hit.transform.TryGetComponent(out IOrderedRenderer ordered))
			{
				SpriteMask decal = Object.Instantiate(damageDecal, ordered.Root);
				decal.transform.position = hit.point;
				decal.frontSortingOrder = ordered.Order + 1;
				decal.backSortingOrder = ordered.Order;
			}
		}
	}
}