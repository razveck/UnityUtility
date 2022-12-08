//Author: João Azuaga

using UnityEngine;

namespace razveck.UnityUtility {
	public static class MathUtility{

		public static float Remap(float value, float fromA, float fromB, float toA, float toB) {
			return Mathf.Lerp(toA, toB, Mathf.InverseLerp(fromA, fromB, value));
		}

	}
}
