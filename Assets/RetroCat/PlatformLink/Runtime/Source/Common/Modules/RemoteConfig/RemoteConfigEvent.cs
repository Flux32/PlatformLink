using System;
using PlatformLink;
using UnityEngine;
using UnityEngine.Events;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig
{
    public class RemoteConfigEvent : MonoBehaviour
    {
        public enum RemoteConfigValueType
        {
            String,
            Int,
            Bool,
            Float,
            Double
        }

        [Serializable]
        public class StringEvent : UnityEvent<string> { }

        [Serializable]
        public class IntEvent : UnityEvent<int> { }

        [Serializable]
        public class BoolEvent : UnityEvent<bool> { }

        [Serializable]
        public class FloatEvent : UnityEvent<float> { }

        [Serializable]
        public class DoubleEvent : UnityEvent<double> { }

        [SerializeField] private string _key;
        [SerializeField] private RemoteConfigValueType _valueType = RemoteConfigValueType.String;

        [SerializeField] private string _fallbackString = string.Empty;
        [SerializeField] private int _fallbackInt;
        [SerializeField] private bool _fallbackBool;
        [SerializeField] private float _fallbackFloat;
        [SerializeField] private double _fallbackDouble;

        [SerializeField] private StringEvent _onStringValue;
        [SerializeField] private IntEvent _onIntValue;
        [SerializeField] private BoolEvent _onBoolValue;
        [SerializeField] private FloatEvent _onFloatValue;
        [SerializeField] private DoubleEvent _onDoubleValue;

        private void OnEnable()
        {
            if (PLink.IsInitialized)
            {
                InvokeEvent();
                return;
            }

            PLink.Initilized += OnInitialized;
        }

        private void OnDisable()
        {
            PLink.Initilized -= OnInitialized;
        }

        private void OnInitialized()
        {
            PLink.Initilized -= OnInitialized;
            InvokeEvent();
        }

        private void InvokeEvent()
        {
            switch (_valueType)
            {
                case RemoteConfigValueType.String:
                    string stringValue = PLink.RemoteConfig.GetRemoteConfig(_key, _fallbackString);
                    _onStringValue?.Invoke(stringValue);
                    break;
                case RemoteConfigValueType.Int:
                    int intValue = PLink.RemoteConfig.GetRemoteConfig(_key, _fallbackInt);
                    _onIntValue?.Invoke(intValue);
                    break;
                case RemoteConfigValueType.Bool:
                    bool boolValue = PLink.RemoteConfig.GetRemoteConfig(_key, _fallbackBool);
                    _onBoolValue?.Invoke(boolValue);
                    break;
                case RemoteConfigValueType.Float:
                    float floatValue = PLink.RemoteConfig.GetRemoteConfig(_key, _fallbackFloat);
                    _onFloatValue?.Invoke(floatValue);
                    break;
                case RemoteConfigValueType.Double:
                    double doubleValue = PLink.RemoteConfig.GetRemoteConfig(_key, _fallbackDouble);
                    _onDoubleValue?.Invoke(doubleValue);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
