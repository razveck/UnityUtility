//Author: João Azuaga

using UniRx;
using UnityEngine;

namespace razveck.UnityUtility {
	public class LoadingScreen : MonoBehaviour {

		[SerializeField]
		private GameObject[] _loadingScreenObjects;

		// Use this for initialization
		private void OnEnable() {
			for(int i = 0; i < _loadingScreenObjects.Length; i++) {
				_loadingScreenObjects[i].SetActive(false);
			}

			SceneService.Instance.LoadingScreenActivated
				.TakeUntilDisable(this)
				.Subscribe(x => {
					for(int i = 0; i < _loadingScreenObjects.Length; i++) {
						_loadingScreenObjects[i].SetActive(x);
					}
				});
		}
	}
}
