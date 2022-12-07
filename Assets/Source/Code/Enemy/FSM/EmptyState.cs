namespace RedRockStudio.SZD.Enemy
{
	public class EmptyState : Stateless.EmptyState, IEnemyState
	{
		public EmptyState(string name) : base(name) { }

		public void EnterFromKneels() { }
	}
}