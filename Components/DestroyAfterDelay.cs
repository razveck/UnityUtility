//Author: João Azuaga

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace razveck.UnityUtility {
	public class DestroyAfterDelay : MonoBehaviour {

		public float Delay;

		// Use this for initialization
		private void Start() {
			Destroy(gameObject, Delay);
		}
	}
}
