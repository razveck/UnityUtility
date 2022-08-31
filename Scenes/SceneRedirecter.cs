//Author: João Azuaga

using UnityEngine;
using UnityEngine.Assertions;

namespace razveck.UnityUtility {
	public class SceneRedirecter : MonoBehaviour {

		[SerializeField]
		private SceneReference[] _scenesToLoad;

		// Use this for initialization
		private async void Start() {
			Assert.IsTrue(_scenesToLoad.Length > 0);
			SceneService.Instance.LoadedScenes.Add(gameObject.scene.path);

			for(int i = 0; i < _scenesToLoad.Length; i++) {
				await SceneService.Instance.LoadSceneAsync(_scenesToLoad[i]);
			}


			await SceneService.Instance.UnloadSceneAsync(gameObject.scene.path);
		}
	}
}
