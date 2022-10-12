//Author: João Azuaga

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace razveck.UnityUtility {
	[RequireComponent(typeof(Camera))]
	public class CustomPhysicsRaycaster : MonoBehaviour {

		private HashSet<GameObject> _enteredObjects = new HashSet<GameObject>();
		private HashSet<GameObject> _objectsToExit = new HashSet<GameObject>();

		private static RaycastHit[] _results;
		private Camera _camera;

		[SerializeField]
		private LayerMask _mask = default;

		[SerializeField]
		private int _numberOfIntersections = default;


		private void Start() {
			_camera = GetComponent<Camera>();
			_results = new RaycastHit[_numberOfIntersections];
		}

		private void Update() {
			foreach(var obj in _enteredObjects) {
				_objectsToExit.Add(obj);
			}
			_enteredObjects.Clear();

			Raycast();

			foreach(var obj in _objectsToExit) {
				if(obj.TryGetComponent(out IPointerExitHandler handler)){
					handler.OnPointerExit(new PointerEventData(EventSystem.current));
				}
			}
			_objectsToExit.Clear();
		}

		public void Raycast() {
			int count = PhysicsUtility.RaycastSorted(_camera.ScreenPointToRay(Input.mousePosition), _results, (int)_camera.farClipPlane, _mask);
			if(count == 0)
				return;

			var pointerData = new PointerEventData(EventSystem.current);
			for(int i = 0; i < count; i++) {

				if(Input.GetMouseButtonDown(0)) {
					var handlers = _results[i].collider.GetComponents<IPointerClickHandler>();
					pointerData.button = PointerEventData.InputButton.Left;
					for(int h = 0; h < handlers.Length; h++) {
						handlers[h].OnPointerClick(pointerData);
					}
				}
				if(Input.GetMouseButtonDown(1)) {
					var handlers = _results[i].collider.GetComponents<IPointerClickHandler>();
					pointerData.button = PointerEventData.InputButton.Right;
					for(int h = 0; h < handlers.Length; h++) {
						handlers[h].OnPointerClick(pointerData);
					}
				}

				var currentGO = _results[i].collider.gameObject;
				var enterHandlers = _results[i].collider.GetComponents<IPointerEnterHandler>();
				for(int h = 0; h < enterHandlers.Length; h++) {
					_objectsToExit.Remove(currentGO);

					if(_enteredObjects.Contains(currentGO))
						continue;

					_enteredObjects.Add(currentGO);
					enterHandlers[h].OnPointerEnter(pointerData);
				}
			}
		}
	}
}
