namespace Extensions.VContainer
{
	public interface IFactory<out TOutput>
	{
		TOutput Create();
	}

	public interface IFactory<in TInput, out TOutput>
	{
		TOutput Create(TInput input);
	}
}