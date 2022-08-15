//Author: João Azuaga

using UnityEngine;

namespace razveck.UnityUtility {
	public class MainMenu : MonoBehaviour {

		[SerializeField]
		private SceneReference[] _scenesToLoad;

		public async void StartGame() {
			for(int i = 0; i < _scenesToLoad.Length; i++) {
				await SceneService.Instance.LoadSceneAsync(_scenesToLoad[i]);
			}

			await SceneService.Instance.UnloadSceneAsync(gameObject.scene.path);
		}

	}
}
