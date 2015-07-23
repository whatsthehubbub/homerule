﻿using SRDebugger.UI.Controls;
using SRF.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace SRDebugger.UI.Other
{
	public class TriggerRoot : SRMonoBehaviourEx
	{

		[RequiredField]
		public Canvas Canvas;

		[RequiredField]
		public RectTransform TriggerTransform;

		[RequiredField]
		[FormerlySerializedAs("TriggerButton")]
		public MultiTapButton TripleTapButton;

		[RequiredField]
		public LongPressButton TapHoldButton;

	}
}
