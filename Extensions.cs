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
			for(int i = 0; i < source.Count; i++) {
				if(i >= start && i <= end) {
					result.Add(source[i]);
				}
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

		#endregion

		#region RectTransform

		/// <summary>
		/// Returns the screen-space rect of the RectTransform.
		/// </summary>
		/// <param name="transform"></param>
		/// <param name="cam"></param>
		/// <returns></returns>
		public static Rect RectTransformToScreenSpace(this RectTransform transform, Camera cam) {
			var worldCorners = new Vector3[4];
			var screenCorners = new Vector3[4];

			transform.GetWorldCorners(worldCorners);

			for(int i = 0; i < 4; i++) {
				screenCorners[i] = cam.WorldToScreenPoint(worldCorners[i]);
			}

			return new Rect(screenCorners[0].x,
							Screen.height - screenCorners[0].y,
							screenCorners[2].x - screenCorners[0].x,
							screenCorners[0].y - screenCorners[2].y);
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
		public static async Task Then(this Task task, Task continuation) {
			await task;
			await continuation;
		}

		/// <summary>
		/// Returns a Task that runs the passed tasks after the current task ends
		/// </summary>
		public static async Task Then(this Task task, params Task[] tasks){
			await task;
			for(int i = 0; i < tasks.Length; i++) {
				await tasks[i];
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
