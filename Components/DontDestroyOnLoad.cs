//Author: João Azuaga

using UnityEngine;

namespace razveck.UnityUtility {
	public class DontDestroyOnLoad : MonoBehaviour {

		// Use this for initialization
		private void Start() {
			DontDestroyOnLoad(gameObject);
		}
	}
}
