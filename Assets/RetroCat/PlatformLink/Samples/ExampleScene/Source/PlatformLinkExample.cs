using UnityEngine;

namespace PlatformLink.Examples
{
    public class PlatformLinkExample : MonoBehaviour
    {
        private void Start()
        {
            PLink.Initialize(null);
            //Debug.Log(PLink.Environment.DeviceType.ToString());
            //Debug.Log(PLink.Environment.Language);
        }
    }
}
