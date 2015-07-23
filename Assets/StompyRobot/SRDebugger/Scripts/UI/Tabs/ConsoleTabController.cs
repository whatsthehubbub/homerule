﻿//#define SR_CONSOLE_DEBUG

using System;
using SRDebugger.Internal;
using SRDebugger.Services;
using SRDebugger.UI.Controls;
using SRF.UI.Layout;
using UnityEngine;
using UnityEngine.UI;

namespace SRDebugger.UI.Tabs
{

	public class ConsoleTabController : SRMonoBehaviourEx
	{

		//public bool IsListening = true;

		[RequiredField] public ScrollRect StackTraceScrollRect;
		[RequiredField] public Text StackTraceText;

		[RequiredField] public Toggle ToggleErrors;
		[RequiredField] public Text ToggleErrorsText;

		[RequiredField] public Toggle ToggleWarnings;
		[RequiredField] public Text ToggleWarningsText;

		[RequiredField] public Toggle ToggleInfo;
		[RequiredField] public Text ToggleInfoText;

		[RequiredField] public Toggle PinToggle;

		[RequiredField]
		public ConsoleLogControl ConsoleLogControl;

		private bool _isDirty;

		protected override void Start()
		{

			base.Start();

			ToggleErrors.onValueChanged.AddListener(isOn => _isDirty = true);
			ToggleWarnings.onValueChanged.AddListener(isOn => _isDirty = true);
			ToggleInfo.onValueChanged.AddListener(isOn => _isDirty = true);

			PinToggle.onValueChanged.AddListener(PinToggleValueChanged);

			ConsoleLogControl.SelectedItemChanged = ConsoleLogSelectedItemChanged;

			Service.Console.Updated += ConsoleOnUpdated;

			StackTraceText.supportRichText = Settings.Instance.RichTextInConsole;
			PopulateStackTraceArea(null);

			Refresh();

		}

		private void PinToggleValueChanged(bool isOn)
		{

			Service.DockConsole.IsVisible = isOn;

		}

		protected override void OnDestroy()
		{

			if (Service.Console != null) {

				Service.Console.Updated -= ConsoleOnUpdated;

			}

			base.OnDestroy();

		}

		protected override void OnEnable()
		{

			base.OnEnable();

			_isDirty = true;

		}

		private void ConsoleLogSelectedItemChanged(object item)
		{

			var log = item as ConsoleEntry;
			PopulateStackTraceArea(log);

		}

		protected override void Update()
		{

			base.Update();

			if(_isDirty)
				Refresh();

		}

		private void PopulateStackTraceArea(ConsoleEntry entry)
		{

			if (entry == null) {

				StackTraceText.text = "";

			} else {

				StackTraceText.text = entry.Message + Environment.NewLine +
				                      (!string.IsNullOrEmpty(entry.StackTrace)
					                      ? entry.StackTrace
					                      : SRDebugStrings.Current.Console_NoStackTrace);

			}

			StackTraceScrollRect.normalizedPosition = Vector2.zero;

		}

		private void Refresh()
		{

			// Update total counts labels
			ToggleInfoText.text = SRDebuggerUtil.GetNumberString(Service.Console.InfoCount, 999, "999+");
			ToggleWarningsText.text = SRDebuggerUtil.GetNumberString(Service.Console.WarningCount, 999, "999+");
			ToggleErrorsText.text = SRDebuggerUtil.GetNumberString(Service.Console.ErrorCount, 999, "999+");

			ConsoleLogControl.ShowErrors = ToggleErrors.isOn;
			ConsoleLogControl.ShowWarnings = ToggleWarnings.isOn;
			ConsoleLogControl.ShowInfo = ToggleInfo.isOn;

			PinToggle.isOn = Service.DockConsole.IsVisible;

			_isDirty = false;

		}

		private void ConsoleOnUpdated(IConsoleService console)
		{
			_isDirty = true;
		}

		public void Clear()
		{
			
			Service.Console.Clear();
			_isDirty = true;

		}

	}

}
