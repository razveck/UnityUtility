//Author: João Azuaga

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace razveck.UnityUtility {
	public class VersionDisplay : MonoBehaviour {

		public TextMeshProUGUI Text;

		// Use this for initialization
		private void Start() {
			Text.text = $"v{Application.version}";
		}
	}
}