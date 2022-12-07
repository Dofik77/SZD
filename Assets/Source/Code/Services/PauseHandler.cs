using RedRockStudio.SZD.Services;
using RedRockStudio.SZD.Services.Input;
using UnityEngine.SceneManagement;

namespace RedRockStudio.SZD.UI
{
	public class PauseHandler : IService
	{
		private readonly IKeysInput _keysInput;

		public PauseHandler(IKeysInput keysInput) =>
			_keysInput = keysInput;

		public void Initialize() =>
			_keysInput.Paused += OnPaused;

		public void Dispose() =>
			_keysInput.Paused += OnPaused;

		private void OnPaused() =>
			SceneManager.LoadScene(0);
	}
}