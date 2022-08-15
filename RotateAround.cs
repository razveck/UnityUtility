//Author: João Azuaga

using UnityEngine;

namespace razveck.UnityUtility {
	public class RotateAround : MonoBehaviour {

		public Vector3 Axis;
		public float Speed = 1;

		private void Update() {
			transform.Rotate(Axis, Speed*Time.deltaTime);
		}
	}
}
