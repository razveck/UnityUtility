//Author: João Azuaga

using System;
using UnityEngine;

namespace razveck.UnityUtility {
	public static class PhysicsUtility {

		public static int RaycastSorted(Ray ray, RaycastHit[] hits, int maxDistance, int layerMask) {
			ClearRaycastArray(hits);
			int hitCount = Physics.RaycastNonAlloc(ray, hits, maxDistance, layerMask);
			SortRaycasts(hits, hitCount);
			return hitCount;
		}

		private static void SortRaycasts(RaycastHit[] hits, int hitCount) {
			Array.Sort(hits, 0, hitCount, RaycastHitDistanceComparer.Instance);
		}

		private static void ClearRaycastArray(RaycastHit[] hits) {
			for(int i = 0; i < hits.Length; i++) {
				hits[i] = default;
			}
		}

	}
}
