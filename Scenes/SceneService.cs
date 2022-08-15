//Author: João Azuaga

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace razveck.UnityUtility {
	public class SceneService : SingletonBehaviour<SceneService> {

		public HashSet<string> LoadedScenes = new HashSet<string>();
		public HashSet<string> LoadingScenes = new HashSet<string>();

		#region Events
		private Subject<bool> _loadingScreenActivated = new Subject<bool>();
		public IObservable<bool> LoadingScreenActivated => _loadingScreenActivated;
		#endregion

		private class LoadingResult {
			public bool Success;
		}

		public async Task LoadSceneAsync(string path, bool showLoadingScreen = false, bool makeActive = true) {
			if(string.IsNullOrEmpty(path))
				return;

			if(LoadedScenes.Contains(path)) {
				Debug.LogWarning($"The scene {path} is already loaded");
				return;
			}

			if(LoadingScenes.Contains(path)) {
				Debug.LogWarning($"The scene {path} is already being loaded");
				return;
			}

			LoadingScenes.Add(path);

			if(showLoadingScreen)
				SetLoadingScreen(true);

			LoadingResult result = new LoadingResult();

			await StartCoroutine(LoadSceneCoroutine(path, result));

			if(result.Success) {
				Scene scene = SceneManager.GetSceneByPath(path);
				if(makeActive)
					SceneManager.SetActiveScene(scene);
			}

			LoadingScenes.Remove(path);
			LoadedScenes.Add(path);

			if(showLoadingScreen)
				SetLoadingScreen(false);
		}

		public async Task UnloadSceneAsync(string path) {
			if(path == null)
				return;

			if(!LoadedScenes.Contains(path)) {
				Debug.LogWarning($"The scene {path} is not loaded");
				return;
			}

			if(LoadingScenes.Contains(path)) {
				Debug.LogWarning($"The scene{path} is already being loaded");
				return;
			}

			LoadingScenes.Add(path);

			LoadingResult result = new LoadingResult();
			await StartCoroutine(UnloadSceneCoroutine(path, result));
			if(!result.Success) {
				return;
			}

			LoadingScenes.Remove(path);
			LoadedScenes.Remove(path);
		}

		public async Task ReloadSceneAsync(string path, bool showLoadingScreen = false) {
			if(showLoadingScreen)
				SetLoadingScreen(true);

			await UnloadSceneAsync(path);
			await LoadSceneAsync(path);

			if(showLoadingScreen)
				SetLoadingScreen(false);
		}

		public void SetLoadingScreen(bool active){
			_loadingScreenActivated.OnNext(active);
		}

		private IEnumerator LoadSceneCoroutine(string path, LoadingResult result) {
			var loading = SceneManager.LoadSceneAsync(path, LoadSceneMode.Additive);
			if(loading == null) {
				Debug.LogError($"Failed to load scene {path} (perhaps it does not exist)");
				result.Success = false;
				yield break;
			}

			while(!loading.isDone)
				yield return null;

			result.Success = true;
		}

		private IEnumerator UnloadSceneCoroutine(string path, LoadingResult result) {
			var unloading = SceneManager.UnloadSceneAsync(path);
			if(unloading == null) {
				Debug.LogError($"Failed to unload scene {path} (perhaps it does not exist)");
				result.Success = false;
				yield break;
			}

			while(!unloading.isDone)
				yield return null;

			result.Success = true;
		}

		protected override void AwakeInternal() {
		}

		protected override void OnDestroyInternal() {
		}

		protected override void OnEnableInternal() {
		}

		protected override void StartInternal() {
		}
	}
}
