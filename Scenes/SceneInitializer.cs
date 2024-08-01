//Author: João Azuaga

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace razveck.UnityUtility {

	/// <summary>
	/// Adds the current scene to the SceneService. Loads scene dependencies on Awake. Also sets the active scene.
	/// Should be added to every scene.
	/// </summary>
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

			Task allScenesTask = Task.CompletedTask;
			for(int i = 0; i < _sceneDependencies.Count; i++) {
				allScenesTask = allScenesTask.CombineWith(SceneService.Instance.LoadSceneAsync(_sceneDependencies[i]));
			}

			await allScenesTask;

			//wait one frame before setting active scene
			await Observable.ReturnUnit().DelayFrame(1);

			if(((string)_activeScene).IsValid())
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
