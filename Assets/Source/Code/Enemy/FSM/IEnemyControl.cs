namespace RedRockStudio.SZD.Enemy
{
	public interface IEnemyControl
	{
		void Kick();

		void Die();

		void Damage();

		void AttackAvaliable();

		void AttackNotAvaliable();
	}
}