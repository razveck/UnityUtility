using UnityEditor;
using UnityEngine;

namespace razveck.UnityUtility.Editor {
	[CustomPropertyDrawer(typeof(SaveGameGUID))]
	public class SaveGameGUIDDrawer : PropertyDrawer{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			var guidProp = property.FindPropertyRelative("_guid");
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("GUID", guidProp.stringValue);
			if(GUILayout.Button("New", GUILayout.ExpandWidth(false))){
				guidProp.stringValue = System.Guid.NewGuid().ToString();
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}