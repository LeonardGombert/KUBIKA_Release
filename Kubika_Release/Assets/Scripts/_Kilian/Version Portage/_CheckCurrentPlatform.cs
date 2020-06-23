using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kubika
{
    public class _CheckCurrentPlatform
    {
        public static RuntimePlatform platform
        {
            get
            {
#if UNITY_ANDROID
                 return RuntimePlatform.Android;
#elif UNITY_IOS
                 return RuntimePlatform.IPhonePlayer;
#elif UNITY_STANDALONE_OSX
                 return RuntimePlatform.OSXPlayer;
#elif UNITY_STANDALONE_WIN
                return RuntimePlatform.WindowsPlayer;
#endif
            }
        }
    }
}
