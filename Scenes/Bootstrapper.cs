//Author: João Azuaga

using UnityEngine;
using UnityEngine.Assertions;

namespace razveck.UnityUtility {
	public class Bootstrapper : MonoBehaviour {

		[SerializeField]
		private SceneReference[] _startupScenes;

		// Use this for initialization
		private async void Start() {
			Assert.IsTrue(_startupScenes.Length > 0);
			SceneService.Instance.LoadedScenes.Add(gameObject.scene.path);

			for(int i = 0; i < _startupScenes.Length; i++) {
				await SceneService.Instance.LoadSceneAsync(_startupScenes[i]);
			}


			await SceneService.Instance.UnloadSceneAsync(gameObject.scene.path);
		}
	}
}
