namespace RedRockStudio.SZD.Character
{
	public class Magazine
	{
		public int Count { get; private set; }

		public Magazine(int capacity) =>
			Count = capacity;

		public void Spend() => 
			Count--;

		public void Reload(int count) => 
			Count = count;
	}
}