﻿using System;
using System.Collections.Generic;
using SRF.UI;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Controls.Data
{

	public class NumberControl : DataBoundControl
	{

		private struct ValueRange
		{

			public double MinValue;
			public double MaxValue;

		}

		[RequiredField]
		public SRNumberSpinner NumberSpinner;

		[RequiredField]
		public Text Title;

		public GameObject[] DisableOnReadOnly;

		private Type _type;

		private static readonly Type[] IntegerTypes = {
			typeof (int), typeof (short), typeof (byte), typeof (SByte), typeof (uint), typeof (ushort)
		};

		private static readonly Type[] DecimalTypes = {
			typeof (float), typeof(double)
		};

		private static readonly Dictionary<Type, ValueRange> ValueRanges = new Dictionary<Type, ValueRange> {
			{typeof (int), new ValueRange() {MaxValue = int.MaxValue, MinValue = int.MinValue}},
			{typeof (short), new ValueRange() {MaxValue = short.MaxValue, MinValue = short.MinValue}},
			{typeof (byte), new ValueRange() {MaxValue = byte.MaxValue, MinValue = byte.MinValue}},
			{typeof (SByte), new ValueRange() {MaxValue = SByte.MaxValue, MinValue = SByte.MinValue}},
			{typeof (uint), new ValueRange() {MaxValue = uint.MaxValue, MinValue = uint.MinValue}},
			{typeof (ushort), new ValueRange() {MaxValue = ushort.MaxValue, MinValue = ushort.MinValue}},
			{typeof (float), new ValueRange() {MaxValue = float.MaxValue, MinValue = float.MinValue}},
			{typeof (double), new ValueRange() {MaxValue = double.MaxValue, MinValue = double.MinValue}}
		};

		private string _lastValue;

		protected override void Start()
		{

			base.Start();
			NumberSpinner.onEndEdit.AddListener(OnValueChanged);

		}

		private void OnValueChanged(string newValue)
		{

			try {

				var num = Convert.ChangeType(newValue, _type);
				UpdateValue(num);

			} catch (Exception) {

				NumberSpinner.text = _lastValue;

			}

		}

		protected override void OnBind(string propertyName, Type t)
		{

			base.OnBind(propertyName, t);
			Title.text = propertyName;

			if (IsIntegerType(t)) {

				NumberSpinner.contentType = InputField.ContentType.IntegerNumber;

			} else if (IsDecimalType(t)) {

				NumberSpinner.contentType = InputField.ContentType.DecimalNumber;

			} else {

				throw new ArgumentException("Type must be one of expected types", "t");

			}

			var rangeAttrib = Property.GetAttribute<SROptions.NumberRangeAttribute>();

			NumberSpinner.MaxValue = GetMaxValue(t);
			NumberSpinner.MinValue = GetMinValue(t);

			if (rangeAttrib != null) {

				NumberSpinner.MaxValue = Math.Min(rangeAttrib.Max, NumberSpinner.MaxValue);
				NumberSpinner.MinValue = Math.Max(rangeAttrib.Min, NumberSpinner.MinValue);

			}

			_type = t;

			NumberSpinner.interactable = !IsReadOnly;

			if (DisableOnReadOnly != null) {

				foreach (var childControl in DisableOnReadOnly) {

					childControl.SetActive(!IsReadOnly);

				}

			}

		}

		protected override void OnValueUpdated(object newValue)
		{

			var value = Convert.ToString(newValue);
			NumberSpinner.text = value;

			_lastValue = value;

		}

		public override bool CanBind(Type type)
		{

			return IsDecimalType(type) || IsIntegerType(type);

		}

		protected static bool IsIntegerType(Type t)
		{

			for (var i = 0; i < IntegerTypes.Length; i++) {
				if (IntegerTypes[i] == t)
					return true;
			}

			return false;

		}

		protected static bool IsDecimalType(Type t)
		{

			for (var i = 0; i < DecimalTypes.Length; i++) {
				if (DecimalTypes[i] == t)
					return true;
			}

			return false;

		}

		protected double GetMaxValue(Type t)
		{

			ValueRange value;
			if (ValueRanges.TryGetValue(t, out value)) {

				return value.MaxValue;

			}

			Debug.LogWarning("[NumberControl] No MaxValue stored for type {0}".Fmt(t));

			return double.MaxValue;

		}

		protected double GetMinValue(Type t)
		{

			ValueRange value;
			if (ValueRanges.TryGetValue(t, out value)) {

				return value.MinValue;

			}

			Debug.LogWarning("[NumberControl] No MinValue stored for type {0}".Fmt(t));

			return double.MinValue;

		}

	}

}