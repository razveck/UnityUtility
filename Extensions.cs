//Author: João Azuaga

using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using UniRx;
using System;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using System.Threading;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace razveck.UnityUtility {
	public static class Extensions {

		#region UnityEngine.Object

		/// <summary>
		/// Uses Destroy or DestroyImmediate depending on the execution context.
		/// </summary>
		/// <param name="obj"></param>
		public static void SafeDestroy(this Object obj) {
			if(Application.isPlaying)
				Object.Destroy(obj);
			else
				Object.DestroyImmediate(obj);
		}

		#endregion

		#region Vector3

		/// <summary>
		/// Returns a <see cref="Vector3"/> with its x, y and z components rounded to the nearest integer
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector3 Round(this Vector3 vec) {
			return new Vector3(Mathf.Round(vec.x), Mathf.Round(vec.y), Mathf.Round(vec.z));
		}

		/// <summary>
		/// Returns a <see cref="Vector3"/> where all components are positive
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector3 Abs(this Vector3 vec) {
			return new Vector3(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z));
		}

		public static Vector2 XZToVector2(this Vector3 vec) {
			return new Vector2(vec.x, vec.z);
		}

		#endregion

		#region Vector2

		/// <summary>
		/// Returns a <see cref="Vector2"/> with its x and y components rounded to the nearest integer
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector2 Round(this Vector2 vec) {
			return new Vector2(Mathf.Round(vec.x), Mathf.Round(vec.y));
		}

		/// <summary>
		/// Returns a <see cref="Vector2"/> where all components are positive
		/// </summary>
		/// <param name="vec"></param>
		/// <returns></returns>
		public static Vector2 Abs(this Vector2 vec) {
			return new Vector2(Mathf.Abs(vec.x), Mathf.Abs(vec.y));
		}

		public static Vector3 ToVector3XZ(this Vector2 vec){
			return new Vector3(vec.x, 0f, vec.y);
		}

		#endregion

		#region Component

		/// <summary>
		/// Gets the component of the specified type in the GameObject or any of its parents, if it exists. Returns true if the component is found, false otherwise.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="current"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool TryGetComponentInParent<T>(this Component current, out T target)
			where T : Component {
			target = current.GetComponentInParent<T>();

			return target != null;
		}

		/// <summary>
		/// Gets the component of the specified type in the GameObject or any of its children, if it exists. Returns true if the component is found, false otherwise.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="current"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool TryGetComponentInChildren<T>(this Component current, out T target)
			where T : Component {
			target = current.GetComponentInChildren<T>();

			return target != null;
		}

		#endregion

		#region GameObject

		/// <summary>
		/// Gets the component of the specified type in the GameObject or any of its parents, if it exists. Returns true if the component is found, false otherwise.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="current"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool TryGetComponentInParent<T>(this GameObject current, out T target)
			 where T : Component {
			target = current.GetComponentInParent<T>();

			return target != null;
		}

		/// <summary>
		/// Gets the component of the specified type in the GameObject or any of its children, if it exists. Returns true if the component is found, false otherwise.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="current"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool TryGetComponentInChildren<T>(this GameObject current, out T target)
			 where T : Component {
			target = current.GetComponentInChildren<T>();

			return target != null;
		}

		/// <summary>
		/// Activates the gameObject if inactive. Deactivates if active.
		/// </summary>
		/// <param name="current"></param>
		public static void ToggleActive(this GameObject current) {
			current.SetActive(!current.activeSelf);
		}

		#endregion

		#region IList<T>

		/// <summary>
		/// Returns a list that is a subsection of the original list.
		/// </summary>
		/// <param name="start">Index where the new list starts [inclusive]</param>
		/// <param name="end">Index where the new list ends [inclusive]</param>
		/// <returns></returns>
		public static IList<T> ExtractRange<T>(this IList<T> source, int start, int end) {
			List<T> result = new List<T>();
			Assert.IsTrue(start < source.Count);
			Assert.IsTrue(end < source.Count);
			for(int i = start; i <= end; i++) {
				result.Add(source[i]);
			}

			return result;
		}

		/// <summary>
		/// Returns a random element of the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T GetRandom<T>(this IList<T> list) {
			return list[Random.Range(0, list.Count)];
		}

		/// <summary>
		/// Removes all null items from the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void ClearNulls<T>(this IList<T> list) {
			for(int i = list.Count - 1; i >= 0; i--) {
				if(list[i] == null)
					list.RemoveAt(i);
			}
		}

		#endregion

		#region IEnumerable<T>

		public static T GetClosestTransform<T>(this IEnumerable<T> list, Transform target)
			where T : Component {
			T closest = null;
			float closestDistance = float.PositiveInfinity;

			foreach(var item in list) {
				float distanceSqr = (item.transform.position - target.position).sqrMagnitude;
				if(distanceSqr < closestDistance) {
					closestDistance = distanceSqr;
					closest = item;
				}
			}

			return closest;
		}

		/// <summary>
		/// Returns a random element of the enumerable.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static T GetRandom<T>(this IEnumerable<T> enumerable) {
			return enumerable.ElementAt(Random.Range(0, enumerable.Count()));
		}

		#endregion

		#region Array

		public static void Clear(this Array array) {
			Array.Clear(array, 0, array.Length);
		}

		#endregion

		#region RectTransform

		private static Vector3[] _worldCorners = new Vector3[4];
		private static Vector3[] _screenCorners = new Vector3[4];

		/// <summary>
		/// Returns the screen-space rect of the RectTransform.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="cam"></param>
		/// <returns></returns>
		public static Rect ScreenSpaceRect(this RectTransform transform, Camera cam) {
			_worldCorners.Clear();
			_screenCorners.Clear();

			transform.GetWorldCorners(_worldCorners);

			var canvas = transform.root.GetComponentInChildren<Canvas>();
			if(canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
				_screenCorners = _worldCorners;
			} else {
				for(int i = 0; i < 4; i++) {
					_screenCorners[i] = cam.WorldToScreenPoint(_worldCorners[i]);
				}
			}
			return new Rect(_screenCorners[0].x,
							Screen.height - _screenCorners[0].y,
							_screenCorners[2].x - _screenCorners[0].x,
							_screenCorners[0].y - _screenCorners[2].y);
		}

		#endregion

		#region string

		/// <summary>
		/// Returns whether the string is not null, empty or white space.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool IsValid(this string value) {
			return !string.IsNullOrEmpty(value) && !string.IsNullOrWhiteSpace(value);
		}

		#endregion

		#region Transform

		/// <summary>
		/// Safely destroys all children of the transform.
		/// </summary>
		/// <param name="transform"></param>
		public static void DestroyChildren(this Transform transform) {
			for(int i = 0; i < transform.childCount; i++) {
				transform.GetChild(i).gameObject.SafeDestroy();
			}
		}

		#endregion

		#region Task

		/// <summary>
		/// Creates a task that combines the current task with a provided task
		/// </summary>
		public static Task CombineWith(this Task task, Task other) {
			return Task.WhenAll(task, other);
		}

		/// <summary>
		/// Creates a task that combines the current task with the provided tasks
		/// </summary>
		public static Task CombineWith(this Task task, params Task[] others) {
			return Task.WhenAll(others.Append(task));
		}

		/// <summary>
		/// Returns a Task that runs the passed task after the current task ends
		/// </summary>
		public static async Task Then(this Task task, Action continuation) {
			await task;
			await Task.Run(continuation);
		}

		/// <summary>
		/// Returns a Task that runs the passed tasks after the current task ends
		/// </summary>
		public static async Task Then(this Task task, params Action[] continuations) {
			await task;
			for(int i = 0; i < continuations.Length; i++) {
				await Task.Run(continuations[i]);
			}
		}

		#endregion

		#region Task<T>
		/// <summary>
		/// Creates a task that combines the current task with a provided task and returns their return values
		/// </summary>
		public static Task<T[]> CombineWith<T>(this Task<T> task, Task<T> other) {
			return Task.WhenAll(task, other);
		}

		/// <summary>
		/// Creates a task that combines the current task with the provided tasks and returns their return values
		/// </summary>
		public static Task<T[]> CombineWith<T>(this Task<T> task, params Task<T>[] others) {
			return Task.WhenAll(others.Append(task));
		}
		#endregion
	}
}
