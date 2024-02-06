//Author: João Azuaga

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace razveck.UnityUtility {
	public class AnimationEventRelay : MonoBehaviour {


		private Subject<bool> _emptyEvent = new Subject<bool>();
		public IObservable<bool> EmptyEvent => _emptyEvent;

		private Subject<float> _floatEvent = new Subject<float>();
		public IObservable<float> FloatEvent => _floatEvent;

		private Subject<int> _intEvent = new Subject<int>();
		public IObservable<int> IntEvent => _intEvent;

		private Subject<string> _stringEvent = new Subject<string>();
		public IObservable<string> StringEvent => _stringEvent;

		private Subject<UnityEngine.Object> _objEvent = new Subject<UnityEngine.Object>();
		public IObservable<UnityEngine.Object> ObjEvent => _objEvent;

		private Subject<(float, int, string, UnityEngine.Object)> _fullEvent = new();
		public IObservable<(float, int, string, UnityEngine.Object)> FullEvent => _fullEvent;

		private void AnimationEvent() {
			_emptyEvent.OnNext(true);
		}

		private void AnimationEventFloat(float value) {
			_floatEvent.OnNext(value);
		}

		private void AnimationEventInt(int value){
			_intEvent.OnNext(value);
		}

		private void AnimationEventString (string value){
			_stringEvent.OnNext(value);
		}

		private void AnimationEventObject(UnityEngine.Object value){
			_objEvent.OnNext(value);
		}

		private void AnimationEventFull(float floatArg, int intArg, string stringArg, UnityEngine.Object objArg) {
			_fullEvent.OnNext((floatArg, intArg, stringArg, objArg));
		}

	}
}
