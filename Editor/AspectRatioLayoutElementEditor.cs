using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace razveck.UnityUtility {
	[CustomEditor(typeof(AspectRatioLayoutElement), true)]
	[CanEditMultipleObjects]
	public class AspectRatioLayoutElementEditor : UnityEditor.Editor {
		
		#region LayoutElement
		SerializedProperty m_IgnoreLayout;
		SerializedProperty m_MinWidth;
		SerializedProperty m_MinHeight;
		SerializedProperty m_PreferredWidth;
		SerializedProperty m_PreferredHeight;
		SerializedProperty m_FlexibleWidth;
		SerializedProperty m_FlexibleHeight;
		SerializedProperty m_LayoutPriority;
		#endregion

		AspectRatioLayoutElement script;
		SerializedProperty aspectRatio;
		SerializedProperty aspectMode;


		protected void OnEnable() {
			script = target as AspectRatioLayoutElement;

			#region LayoutElement
			m_IgnoreLayout = serializedObject.FindProperty("m_IgnoreLayout");
			m_MinWidth = serializedObject.FindProperty("m_MinWidth");
			m_MinHeight = serializedObject.FindProperty("m_MinHeight");
			m_PreferredWidth = serializedObject.FindProperty("m_PreferredWidth");
			m_PreferredHeight = serializedObject.FindProperty("m_PreferredHeight");
			m_FlexibleWidth = serializedObject.FindProperty("m_FlexibleWidth");
			m_FlexibleHeight = serializedObject.FindProperty("m_FlexibleHeight");
			m_LayoutPriority = serializedObject.FindProperty("m_LayoutPriority");
			#endregion


			aspectRatio = serializedObject.FindProperty("_aspectRatio");
			aspectMode = serializedObject.FindProperty("_aspectMode");
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			#region LayoutElement
			//serializedObject.Update();

			EditorGUILayout.PropertyField(m_IgnoreLayout);

			if(!m_IgnoreLayout.boolValue) {
				EditorGUILayout.Space();

				//my stuff
				EditorGUILayout.PropertyField(aspectMode);
				EditorGUILayout.PropertyField(aspectRatio);
				//--------

				LayoutElementField(m_MinWidth, 0, script.aspectMode == AspectRatioLayoutElement.AspectMode.WidthControlsHeight);
				LayoutElementField(m_MinHeight, 0, script.aspectMode == AspectRatioLayoutElement.AspectMode.HeightControlsWidth);
				LayoutElementField(m_PreferredWidth, t => t.rect.width, script.aspectMode == AspectRatioLayoutElement.AspectMode.WidthControlsHeight);
				LayoutElementField(m_PreferredHeight, t => t.rect.height, script.aspectMode == AspectRatioLayoutElement.AspectMode.HeightControlsWidth);
				LayoutElementField(m_FlexibleWidth, 1, true);
				LayoutElementField(m_FlexibleHeight, 1, true);
			}

			EditorGUILayout.PropertyField(m_LayoutPriority);
			//serializedObject.ApplyModifiedProperties();
			#endregion



			if(GUILayout.Button("Reset")) {
				script.ResetToInitial();
			}

			serializedObject.ApplyModifiedProperties();
		}

		void LayoutElementField(SerializedProperty property, float defaultValue, bool canBeEnabled) {
			LayoutElementField(property, _ => defaultValue, canBeEnabled);
		}

		void LayoutElementField(SerializedProperty property, System.Func<RectTransform, float> defaultValue, bool canBeEnabled) {
			Rect position = EditorGUILayout.GetControlRect();

			EditorGUI.BeginDisabledGroup(!canBeEnabled);
			// Label
			GUIContent label = EditorGUI.BeginProperty(position, null, property);

			// Rects
			Rect fieldPosition = EditorGUI.PrefixLabel(position, label);

			Rect toggleRect = fieldPosition;
			toggleRect.width = 16;

			Rect floatFieldRect = fieldPosition;
			floatFieldRect.xMin += 16;

			// Checkbox
			EditorGUI.BeginChangeCheck();
			bool enabled = EditorGUI.ToggleLeft(toggleRect, GUIContent.none, property.floatValue >= 0);
			if(EditorGUI.EndChangeCheck()) {
				// This could be made better to set all of the targets to their initial width, but mimizing code change for now
				property.floatValue = (enabled ? defaultValue((target as LayoutElement).transform as RectTransform) : -1);
			}

			if(!property.hasMultipleDifferentValues && property.floatValue >= 0) {
				// Float field
				EditorGUIUtility.labelWidth = 4; // Small invisible label area for drag zone functionality
				EditorGUI.BeginChangeCheck();
				float newValue = EditorGUI.FloatField(floatFieldRect, new GUIContent(" "), property.floatValue);
				if(EditorGUI.EndChangeCheck()) {
					property.floatValue = Mathf.Max(0, newValue);
				}
				EditorGUIUtility.labelWidth = 0;
			}



			EditorGUI.EndProperty();
			EditorGUI.EndDisabledGroup();
		}

	}
}
