//Author: João Azuaga

using UnityEngine;

namespace razveck.UnityUtility {
	[DefaultExecutionOrder(-1000)]
	public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T> {

		public static T Instance;

		private void Awake() {
			if(Instance != null) {
				gameObject.SafeDestroy();
				return;
			}

			Instance = (T)this;
			transform.SetParent(null);
			DontDestroyOnLoad(this);
			Debug.Log($"{Instance.GetType()} initialized.");
			AwakeInternal();
		}

		protected virtual void AwakeInternal() { }

		protected void OnEnable() {
			if(Instance != this) {
				return;
			}

			OnEnableInternal();
		}

		protected virtual void OnEnableInternal() { }

		private void Start() {
			if(Instance != this) {
				return;
			}

			StartInternal();
		}

		protected virtual void StartInternal() { }

		private void OnDestroy() {
			if(Instance != this) {
				return;
			}

			OnDestroyInternal();
		}

		protected virtual void OnDestroyInternal() { }
	}
}
