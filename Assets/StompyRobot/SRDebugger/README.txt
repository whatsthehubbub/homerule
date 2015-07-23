=====================
SRDebugger - (C) Stompy Robot LTD 2015
=====================

Visit https://www.stompyrobot.uk/tools/srdebugger/documentation for more detailed documentation.

Getting Started:
--------

UNITY 5
---

No setup required. SRDebugger will automatically load at runtime unless "Auto Load" is disabled in settings. Triple-tap in the top-left corner of the game view (by default) to open the debug panel.

If Auto Load is disabled, follow the instructions for Unity 4.

UNITY 4.6+
---

- Drag the SRDebugger.Init prefab from the SRDebugger folder into your scene. Done! By default, the trigger to open the debug panel is attached to the top-left of the screen. Triple-tap there to open the panel.

- Recommended usage is to put the SRDebugger.Init prefab in the first scene that your game loads. This way, the debugger can pick up the maximum number of console messages from the initialisation stage of your game, and will be available to any subsequent scenes. Due to limitations in Unity, console messages from before SRDebugger loads are unable to be gathered.

SHARED
---

- Navigate to "Window/SRDebugger Settings" to open the settings pane for SRDebugger. You can set up trigger behaviour, pin entry, and a global enable toggle here.

- Open StompyRobot/SRDebugger/Scenes/Sample.unity for a simple example scene.

Advanced Features:
--------

- To use the options pane, define a class with the following signature:

		public partial class SROptions { }

Any properties of the supported types defined in this class will appear in the SRDebugger Options pane.
Public methods with no parameters that return void will appear as actions in the options pane.
Use the CategoryAttribute to organise the options screen. Options will be grouped by category name. CategoryAttribute is located in the System.ComponentModel namespace.

Example:

		using namespace System.ComponentModel;

		public partial class SROptions {
			
			private float _myProperty = 0.5f; // Default Value
			
			[Category("My Category")]
			public float MyProperty {
				get { return _myProperty; }
				set { _myProperty = value; }
			}
			
			private float _myRangeProperty = 0f; // Default Value
			
			[NumberRange(0, 1)] // The NumberRange attribute will ensure that the value never leaves the range 0-1
			[Category("My Category")]
			public float MyRangeProperty {
				get { return _myRangeProperty; }
				set { _myRangeProperty = value; }
			}
			
		}

To use these properties in your code, access the properties via the Current static property on SROptions.

		SROptions.Current.MyProperty;
		
For more examples, look in StompyRobot/SRDebugger/Scripts/SROptions.Test.cs

Options can be pinned to the game view by pressing the Pin button when on the options tab and selecting the options you wish to appear. These options can then be modified without entering the debug panel.

Using the Sort attribute you can specify the order in which options will appear within their category. (Default sorting position is 0 for all options. Negative indexes are allowed)

	[Sort(-1)] // Appear first in category
	[Category("My Category")]
	public float MySortedProperty {
		get { return _mySortedProperty; }
		set { _mySortedProperty = value; }
	}
	
If you use the OnPropertyChanged() callback any changes to your properties will be reflected in pinned options immediately, instead of only when the debug panel open/closes.

	public float MyNotifyingProperty {
		get { return _myNotifyingProperty; }
		set { 
			_myNotifyingProperty = value; 
			OnPropertyChanged("MyNotifyingProperty");
		}
	}

- To use the PIN entry system for your own code, do something like this:

		// Show a pin entry form which requires the pin "1234" to succeed.
		SRF.Service.SRServiceManager.GetService<SRDebugger.Services.IPinEntryService>().ShowPinEntry(new[] {1, 2, 3, 4}, "Prompt Message",
			success => {

				if(success) {
					// User entered correct PIN
				}

			});
			

Visit www.stompyrobot.uk/tools/srdebugger/documentation for more information.

Bug Reporter
--------

To use the Bug Reporter you must use the signup form in the settings menu to generate an API key. Once this is done, and you have verified your email address, you can
receive bug reports to your registered email address.

- On Windows 8, ensure that the "Internet Client" capability is granted to your application in Player Settings
- You can open a bug report popover by called SRDebug.Instance.ShowBugReportSheet(...) (use intellisense for parameters)

Troubleshooting:
--------

- Q: My game responds to input while the debug panel is open. How can I stop this?
  A: Use the IsPointerOverGameObject() method to filter input that is blocked by a UI element. (See http://forum.unity3d.com/threads/frequently-asked-ui-questions.264479/ "How do I stop clicks/touches on the UI from 'going through it' and being clicks in my game world?")


Restrictions:
--------

 - Icons included in this pack must only be used in the SRDebugger panel. If you wish to use the icons outside of the debug panel, consider licensing from icons8.com/buy
 - Unauthorised distribution of this library is not permitted. See Unity Asset Store EULA for details.
 
Credits:
--------

- Programming/Design by Simon Moles @ Stompy Robot (simon@stompyrobot.uk, www.stompyrobot.uk)
- Icons provided by Icons8 (www.icons8.com)
- Side-bar background pattern provided by Subtle Patterns (www.subtlepatterns.com)
- Orbitron font provided by the League of Moveable Type (theleagueofmoveabletype.com) (Open Font License 1.1)
- Source Code Pro font provided by Adobe (github.com/adobe-fonts/source-code-pro) (Open Font License 1.1)


Change Log:
--------

1.2.1
----------

New:
- Added DisplayName attribute for use with SROptions.

Changes:
- Read-only properties are now added to options tab (but can't be modified).
- Sort attribute can now be applied to methods.

Fixes:
- Fixed compile errors when NGUI is imported in the same project.
- Removed excess logging when holding a number button in options tab.

1.2.0
----------

New:
- Dock console at the top of the screen. (open from the console tab, SRDebug API or keyboard shortcuts)
- Collapse duplicate log entries (enable in settings)
- Bug Report popover. Show bug reporter without granting access to the debug panel. Open via keyboard shortcut or the SRDebug API.
- Added Sort attribute to sort items in options tab. (See SROptions.Test.cs for examples)
- Added SROptions PropertyChanged support. Call OnPropertyChanged() in your setters and pinned options will update to reflect the new value.
- Entry code can now be entered with keyboard.

Changes:
- Sending screenshot with bug report now supported on web player.

Fixes:
- Fixed pin entry canvas not using correct UI camera.
- Modified namespaces and naming of internal classes to reduce conflicts with other assets.
- Fixed script updater having to run for Unity 5.1
- Misc bug fixes

1.1.2
----------

Changes:
- Bug reporter is now supported on Web Player builds (now uses Unity WWW instead of HttpWebRequest for API calls)
- System Information area now shows IL2CPP status on iOS builds
- Application.platform value is now included with bug reports
- Support for Unity 5.1

Fixes:
- Fixed issues with options panel and IL2CPP on iOS
- Unity Cloud Build information now formatted correctly
- Fixed Settings UI issue on Unity 5.1 beta
- Fixed Entry Code setting having no effect
- Fixed keyboard shortcuts bypassing entry code if enabled

1.1.1
----------

Changes:
- The version of SRF (https://github.com/StompyRobot/SRF) has been changed to the "Lite" version, containing only scripts relevant to SRDebugger. If you want the full SRF library it is available free on GitHub.

Fixes:
- SRDebugger no longer creates an event system in a scene if one already exists on Unity 5 using Auto-Init.
- Fixed CategoryAttribute being in the wrong namespace when when compiling for Windows 8 platforms.

1.1.0
----------

New:
- (Unity 5) Can enable "Auto-Init" in the Settings pane to automatically initialize SRDebugger without SRDebugger.Init prefab included in the scene.
- (BETA) Bug Reporter - Users can submit bug reports, with console log and system information included. These will be forwarded to you by email. (Enable in Settings)
- (BETA) Windows Store support
- Added support for Keyboard Shortcuts
- Added Trigger Behaviour option. Switch between "Triple-Tap" and "Tap-And-Hold" methods for opening debug panel
- Added Default Tab option in Settings pane
- Added Layer option to settings panel to choose which layer UI will be on
- Added Debug Camera mode (render debug panel UI to a camera instead of overlay)
- SRDebug.Init() method added for custom initialisation of SRDebugger without SRDebugger.Init prefab
- Event added to SRDebug on panel open/close

Changes:
- Scroll sensitivity has been improved for desktop platforms

1.0.2
----------

Fixed:
- Fixed console layout with Unity 4.6.3+
- Trigger Position setting now checked on init

1.0.1
----------

New:
- Unity 5.0 Support.
- Added option to Settings pane to require the entry code for every time the panel opens, instead of just the first time.

Fixed:
- Removed debug message when opening Options tab for first time.
- Fixed conflict with NGUI RealTime class.
- Fixed layout of pinned options when number of items exceeds screen width.

1.0.0
----------

Initial version.
