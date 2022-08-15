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
	public class SceneContext : MonoBehaviour {
		private int _cameraIndex;
		private Camera[] _cameras;

		[SerializeField]
		private List<SceneReference> _sceneDependencies;
		[SerializeField]
		private SceneReference _activeScene;

		public Camera ActiveCamera { get; private set; }

		#region Events
		private ReplaySubject<Camera> _activeCameraChanges = new ReplaySubject<Camera>(1);
		public IObservable<Camera> ActiveCameraChanges => _activeCameraChanges;
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

		public static SceneContext GetContextForObject(GameObject caller){
			var gos = caller.scene.GetRootGameObjects();
			foreach(var go in gos) {
				if(go.TryGetComponent(out SceneContext context)){
					return context;
				}
			}

			throw new MissingComponentException($"There is no {nameof(SceneContext)} in {caller.name}'s scene");
		}

		public static T GetContextForObject<T>(GameObject caller) where T : ContextBehaviour<T>{
			var gos = caller.scene.GetRootGameObjects();
			foreach(var go in gos) {
				if(go.TryGetComponent(out T context)){
					return context;
				}
			}

			throw new MissingComponentException($"There is no {nameof(ContextBehaviour<T>)} in {caller.name}'s scene");
		}

		public void SetCameras(IEnumerable<Camera> cameras) {
			_cameras = cameras.ToArray();
			SwitchCamera(0);
		}

		public void SwitchCamera(int direction) {
			_cameraIndex = (int)Mathf.Repeat(_cameraIndex + direction, _cameras.Length);
			ActiveCamera = _cameras[_cameraIndex];
			_activeCameraChanges.OnNext(ActiveCamera);
		}
	}
}
