//Author: João Azuaga

using Unity.Mathematics;
using UnityEngine;

namespace razveck.UnityUtility {
	public static class MathUtility {

		public static float Remap(float value, float fromA, float fromB, float toA, float toB) {
			return Mathf.Lerp(toA, toB, Mathf.InverseLerp(fromA, fromB, value));
		}

		public static float LerpSmooth(float a, float b, float slope, float deltaTime) {
			return b + (a - b) * math.exp(-slope * deltaTime);
		}
	}
}
