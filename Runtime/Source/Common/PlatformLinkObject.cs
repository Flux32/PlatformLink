using System;
using UnityEngine;

namespace PlatformLink
{
    public static class PlatformLinkObject
    {
        private const string ObjectName = "#!_platform_link_#!";
        private static GameObject _object;

        public static void Initialize()
        {
            if (_object != null)
                throw new InvalidOperationException("PlatformLinkObject is already initialized.");
            
            _object = new GameObject(ObjectName);
            UnityEngine.Object.DontDestroyOnLoad(_object);
        }

        public static T AddComponent<T>() where T : Component
        {
            if (_object.GetComponent<T>() == null)
            {
                return _object.AddComponent<T>();
            }
            
            throw new InvalidOperationException("Component is already exists");
        }

        public static void ClearInstance()
        {
            _object = null;
        }
    }
}
