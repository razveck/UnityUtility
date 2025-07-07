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
	public class SetTimeScale : MonoBehaviour {

		public float TimeScale = 1f;

		// Use this for initialization
		private void Start() {
			Time.timeScale = TimeScale;
		}
	}
}
