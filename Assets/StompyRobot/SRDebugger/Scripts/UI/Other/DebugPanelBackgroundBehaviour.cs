﻿using SRF.UI;
using UnityEngine;

namespace SRDebugger.UI.Other
{

	[RequireComponent(typeof(StyleComponent))]
	public class DebugPanelBackgroundBehaviour : SRMonoBehaviour
	{

		private StyleComponent _styleComponent;

		public string TransparentStyleKey = "";
		private string _defaultKey;

		private bool _isTransparent;

		private void Awake()
		{

			_styleComponent = GetComponent<StyleComponent>();

			_defaultKey = _styleComponent.StyleKey;
			Update();

		}

		private void Update()
		{

			if (!_isTransparent && Settings.Instance.EnableBackgroundTransparency) {

				_styleComponent.StyleKey = TransparentStyleKey;
				_isTransparent = true;

			} else if (_isTransparent && !Settings.Instance.EnableBackgroundTransparency) {

				_styleComponent.StyleKey = _defaultKey;
				_isTransparent = false;

			}

		}

	}
}