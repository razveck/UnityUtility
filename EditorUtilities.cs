#if UNITY_EDITOR
//Author: João Azuaga

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace razveck.UnityUtility {
	public static class EditorUtilities {

		public static T LoadAssetOfType<T>() where T : UnityEngine.Object {
			Type t = typeof(T);
			Debug.Log(t);
			T result = AssetDatabase.FindAssets($"t:{t}")
				.Select(x => AssetDatabase.GUIDToAssetPath(x))
				.Select(x => AssetDatabase.LoadAssetAtPath<T>(x))
				.First();

			return result;
		}

		public static List<T> LoadAssetsOfType<T>() where T : UnityEngine.Object {
			Type t = typeof(T);
			Debug.Log(t);
			List<T> result = AssetDatabase.FindAssets($"t:{t}")
				.Select(x => AssetDatabase.GUIDToAssetPath(x))
				.Select(x => AssetDatabase.LoadAssetAtPath<T>(x))
				.ToList();

			return result;
		}
	}
}
#endif