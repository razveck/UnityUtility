//Author: João Azuaga

using UnityEngine;
using UnityEngine.EventSystems;

namespace razveck.UnityUtility {
	public static class InputUtility {

		public static bool IsOverUI() {
			bool isOverUI = false;
#if UNITY_EDITOR
			isOverUI = EventSystem.current.IsPointerOverGameObject();
#elif UNITY_IOS || UNITY_ANDROID
			isOverUI = Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId);
#else
			isOverUI = EventSystem.current.IsPointerOverGameObject();
#endif

			return isOverUI;
		}

	}
}
