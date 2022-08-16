//Author: João Azuaga

using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace razveck.UnityUtility {
	public class FPSCounter : MonoBehaviour {


		[SerializeField]
		private TextMeshProUGUI _textField;
		[SerializeField]
		private bool _updateEveryFrame;
		[SerializeField]
		[HideIf(nameof(_updateEveryFrame))]
		private float _updateInterval;

		// Update is called once per frame
		private IEnumerator Start() {
			var seconds = new WaitForSeconds(_updateInterval);

			while(true) {
				_textField.text = (Mathf.Round(1 / Time.deltaTime)).ToString();
				if(_updateEveryFrame)
					yield return null;
				else
					yield return seconds;
			}
		}
	}
}
