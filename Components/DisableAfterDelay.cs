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
	public class DisableAfterDelay : MonoBehaviour {

		private float _timer = 0f;

		public float Delay;

		//[Tooltip("Should this disable itself every single time it gets enabled?")]
		//public bool EveryTime;

		private void OnEnable() {
			_timer = 0f;
		}

		// Update is called once per frame
		private void Update() {
			_timer += Time.deltaTime;
			if(_timer >= Delay) {
				gameObject.SetActive(false);
			}
		}
	}
}
