//Author: João Azuaga

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace razveck.UnityUtility {
	public class AspectRatioLayoutElement : LayoutElement {

		/// <summary>
		/// Specifies a mode to use to enforce an aspect ratio.
		/// </summary>
		public enum AspectMode {
			/// <summary>
			/// The aspect ratio is not enforced
			/// </summary>
			None,
			/// <summary>
			/// Changes the height of the rectangle to match the aspect ratio.
			/// </summary>
			WidthControlsHeight,
			/// <summary>
			/// Changes the width of the rectangle to match the aspect ratio.
			/// </summary>
			HeightControlsWidth,
			/// <summary>
			/// Sizes the rectangle such that it's fully contained within the parent rectangle.
			/// </summary>
			FitInParent,
			/// <summary>
			/// Sizes the rectangle such that the parent rectangle is fully contained within.
			/// </summary>
			EnvelopeParent
		}

		private float _initialAspectRatio;

		[SerializeField] private AspectMode _aspectMode = AspectMode.None;

		/// <summary>
		/// The mode to use to enforce the aspect ratio.
		/// </summary>
		public AspectMode aspectMode { get { return _aspectMode; } set { if(SetStruct(ref _aspectMode, value)) CalculateSize(); } }

		[SerializeField] private float _aspectRatio = 1;

		/// <summary>
		/// The aspect ratio to enforce. This means width divided by height.
		/// </summary>
		public float aspectRatio { get { return _aspectRatio; } set { if(SetStruct(ref _aspectRatio, value)) CalculateSize(); } }

		[System.NonSerialized]
		private RectTransform m_Rect;

		//Does the gameobject has a parent for reference to enable FitToParent/EnvelopeParent modes.
		private bool m_DoesParentExist = false;

		public override float preferredHeight
		{
			get
			{
				return base.preferredHeight;
			}
			set
			{
				base.preferredHeight = value;
			}
		}

		private RectTransform rectTransform
		{
			get
			{
				if(m_Rect == null)
					m_Rect = GetComponent<RectTransform>();
				return m_Rect;
			}
		}

		private float GetSizeDeltaToProduceSize(float size, int axis) {
			return size - GetParentSize()[axis] * (rectTransform.anchorMax[axis] - rectTransform.anchorMin[axis]);
		}

		private Vector2 GetParentSize() {
			RectTransform parent = rectTransform.parent as RectTransform;
			return !parent ? Vector2.zero : parent.rect.size;
		}

		protected void CalculateSize() {
			switch(_aspectMode) {
#if UNITY_EDITOR
				case AspectMode.None: {
					if(!Application.isPlaying) {
						_initialAspectRatio = Mathf.Clamp(rectTransform.rect.width / rectTransform.rect.height, 0.001f, 1000f);
						if(_aspectRatio == -999)
							_aspectRatio = _initialAspectRatio;
					}

					break;
				}
#endif
				case AspectMode.HeightControlsWidth: {
					preferredWidth = rectTransform.rect.height * _aspectRatio;
					break;
				}
				case AspectMode.WidthControlsHeight: {
					preferredHeight = rectTransform.rect.width / _aspectRatio;
					break;
				}
				case AspectMode.FitInParent:
				case AspectMode.EnvelopeParent: {
					if(!DoesParentExist())
						break;

					rectTransform.anchorMin = Vector2.zero;
					rectTransform.anchorMax = Vector2.one;
					rectTransform.anchoredPosition = Vector2.zero;

					Vector2 sizeDelta = Vector2.zero;
					Vector2 parentSize = GetParentSize();
					if((parentSize.y * aspectRatio < parentSize.x) ^ (_aspectMode == AspectMode.FitInParent)) {
						sizeDelta.y = GetSizeDeltaToProduceSize(parentSize.x / aspectRatio, 1);
					} else {
						sizeDelta.x = GetSizeDeltaToProduceSize(parentSize.y * aspectRatio, 0);
					}
					rectTransform.sizeDelta = sizeDelta;

					break;
				}
			}

			SetDirty();
		}

		protected override void OnEnable() {
			CalculateSize();
		}

		public bool IsAspectModeValid() {
			if(!DoesParentExist() && (aspectMode == AspectMode.EnvelopeParent || aspectMode == AspectMode.FitInParent))
				return false;

			return true;
		}

		private bool DoesParentExist() {
			return m_DoesParentExist;
		}

#if UNITY_EDITOR
		protected override void OnValidate() {
			_aspectRatio = Mathf.Clamp(_aspectRatio, 0.001f, 1000f);
			CalculateSize();
		}

#endif

		static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct {
			if(EqualityComparer<T>.Default.Equals(currentValue, newValue))
				return false;

			currentValue = newValue;
			return true;
		}

		public void ResetToInitial() {
			aspectRatio = _initialAspectRatio;
		}
	}
}
