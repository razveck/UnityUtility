//Author: João Azuaga

using System.Collections.Generic;
using UnityEngine;

namespace razveck.UnityUtility {
	public class RaycastHitDistanceComparer : IComparer<RaycastHit> {

		public readonly static RaycastHitDistanceComparer Instance;

		static RaycastHitDistanceComparer() {
			Instance = new RaycastHitDistanceComparer();
		}

		public int Compare(RaycastHit a, RaycastHit b) {
			if(a.distance < b.distance)
				return -1;
			
			return 1;
		}
	}
}
