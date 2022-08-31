//Author: João Azuaga

using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace razveck.UnityUtility {
	public class SceneInitializer : MonoBehaviour {
		[SerializeField]
		private List<SceneReference> _sceneDependencies;
		[SerializeField]
		private SceneReference _activeScene;

		#region Events
		#endregion

		protected async void Awake() {
			if(!SceneService.Instance.LoadedScenes.Contains(gameObject.scene.path)) {
				SceneService.Instance.LoadedScenes.Add(gameObject.scene.path);
			}
			for(int i = 0; i < _sceneDependencies.Count; i++) {
				await SceneService.Instance.LoadSceneAsync(_sceneDependencies[i]);
			}

			await Observable.ReturnUnit().DelayFrame(1);

			if(!string.IsNullOrEmpty(_activeScene))
				SceneManager.SetActiveScene(SceneManager.GetSceneByPath(_activeScene));
		}

		public static SceneInitializer GetInitializerForObject(GameObject caller){
			var gos = caller.scene.GetRootGameObjects();
			foreach(var go in gos) {
				if(go.TryGetComponent(out SceneInitializer initializer)){
					return initializer;
				}
			}

			throw new MissingComponentException($"There is no {nameof(SceneInitializer)} in {caller.name}'s scene");
		}
	}
}
