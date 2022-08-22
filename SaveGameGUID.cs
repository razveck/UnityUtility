//Author: João Azuaga

using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace razveck.UnityUtility {
	[Serializable]
	public struct SaveGameGUID {
		[SerializeField]
		[InlineButton(nameof(NewGUID), "New")]
		[ReadOnly]
		private string _guid;
		public string Guid => _guid;

		public void NewGUID() {
			_guid = System.Guid.NewGuid().ToString();
		}

		public override string ToString() {
			return _guid;
		}

		public static implicit operator string(SaveGameGUID id){
			return id._guid;
		}
	}
}
