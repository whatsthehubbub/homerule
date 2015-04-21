
using UnityEngine;

namespace CameraShot
{

	public class AndroidCameraShot
	{
		#if UNITY_ANDROID
		static AndroidJavaClass _plugin;

		static AndroidCameraShot()
		{
			_plugin = new AndroidJavaClass("com.astricstore.camerashots.CameraShots");
		}

		public static void LaunchCameraForImageCapture()
		{
			CameraShot.mode = 0;
			LaunchCameraForImage ();
		}
		
		public static void GetTexture2DFromCamera()
		{
			CameraShot.mode = 1;
			LaunchCameraForImage ();
		}
		

		// for video
		public static void LaunchCameraForVideoCapture()
		{
			LaunchCameraForVideo(0);
		}

		public static void LaunchCameraForVideoCapture(int maxDuration)
		{
			LaunchCameraForVideo(maxDuration);
		}


		private static void LaunchCameraForImage()
		{
			_plugin.CallStatic("launchCameraForImageCapture");

		}

		private static void LaunchCameraForVideo(int maxDuration)
		{
			_plugin.CallStatic("launchCameraForVideoCapture",maxDuration);

		}
#endif
	}
}


