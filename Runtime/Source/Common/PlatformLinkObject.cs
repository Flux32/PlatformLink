using System;
using UnityEngine;

namespace PlatformLink
{
    public static class PlatformLinkObject
    {
        private const string ObjectName = "#!_platform_link_#!";
        private static readonly GameObject _object = new GameObject(ObjectName);

        public static void Initialize()
        {
            UnityEngine.Object.DontDestroyOnLoad(_object);
        }

        public static T AddComponent<T>() where T : Component
        {
            if (_object.GetComponent<T>() == null)
            {
                return _object.AddComponent<T>();
            }
            else
            {
                throw new InvalidOperationException("Component is already exists");
            }
        }
    }
}
