﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;
using SRF.UI;

namespace SRDebugger.UI.Controls.Data
{

	public class EnumControl : DataBoundControl
	{

		[RequiredField]
		public Text Title;

		[RequiredField]
		public Text Value;

		[RequiredField]
		public LayoutElement ContentLayoutElement;

		[RequiredField]
		public SRSpinner Spinner;

		public GameObject[] DisableOnReadOnly;

		private string[] _names;
		private Array _values;

		private object _lastValue;

		protected override void Start()
		{
			base.Start();
		}

		protected override void OnBind(string propertyName, Type t)
		{

			base.OnBind(propertyName, t);

			Title.text = propertyName;

			Spinner.interactable = !IsReadOnly;

			if (DisableOnReadOnly != null) {

				foreach (var child in DisableOnReadOnly) {

					child.SetActive(!IsReadOnly);

				}

			}

			_names = Enum.GetNames(t);
			_values = Enum.GetValues(t);

			var longestName = "";

			for (var i = 0; i < _names.Length; i++) {

				if (_names[i].Length > longestName.Length)
					longestName = _names[i];

			}

			if (_names.Length == 0)
				return;

			// Set preferred width of content to the largest possible value size

			var width = Value.cachedTextGeneratorForLayout.GetPreferredWidth(longestName,
				Value.GetGenerationSettings(new Vector2(float.MaxValue, Value.preferredHeight)));

			ContentLayoutElement.preferredWidth = width;

		}

		protected override void OnValueUpdated(object newValue)
		{

			_lastValue = newValue;
			Value.text = newValue.ToString();

		}

		public override bool CanBind(Type type)
		{
#if NETFX_CORE
			return type.GetTypeInfo().IsEnum;
#else
			return type.IsEnum;
#endif
		}

		private void SetIndex(int i)
		{

			UpdateValue(_values.GetValue(i));
			Refresh();

		}

		public void GoToNext()
		{

			var currentIndex = Array.IndexOf(_values, _lastValue);
			SetIndex(SRMath.Wrap(_values.Length, currentIndex+1));

		}

		public void GoToPrevious()
		{

			var currentIndex = Array.IndexOf(_values, _lastValue);
			SetIndex(SRMath.Wrap(_values.Length, currentIndex - 1));

		}

	}

}