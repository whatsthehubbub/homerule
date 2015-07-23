﻿using System;
using System.Diagnostics;
using UnityEngine;

namespace SRDebugger.Profiler
{

	[RequireComponent(typeof(Camera))]
	public class ProfilerCameraListener : MonoBehaviour
	{

		private Camera _camera;

		private Stopwatch _stopwatch;

		public Camera Camera
		{
			get { return _camera; }
		}

		public Action<ProfilerCameraListener, double> RenderDurationCallback; 

		private void Awake()
		{
			_camera = GetComponent<Camera>();
			_stopwatch = new Stopwatch();
		}

		private void OnPreCull()
		{

			_stopwatch.Start();

		}

		private void OnPostRender()
		{

			var renderTime = _stopwatch.Elapsed.TotalSeconds;

			_stopwatch.Stop();
			_stopwatch.Reset();

			if (RenderDurationCallback != null)
				RenderDurationCallback(this, renderTime);

		}

	}

}