using UnityEngine;
using UnityEngine.UI;

namespace PlatformLink.Examples
{
    public class PlatformLinkSaveExample : MonoBehaviour
    {
        [SerializeField] private InputField _inputTextField;
        [SerializeField] private InputField _inputKeyField;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;

        private const string SavedMessage = "Text saved successfully";

        private void OnEnable()
        {
            _saveButton.onClick.AddListener(OnSaveButtonClicked);
            _loadButton.onClick.AddListener(OnLoadButtonClicked);
        }

        private void OnDisable()
        {
            _saveButton.onClick.RemoveListener(OnSaveButtonClicked);
            _loadButton.onClick.RemoveListener(OnLoadButtonClicked);
        }

        private void OnSaveButtonClicked()
        {
            PLink.Storage.Save(_inputKeyField.text, _inputTextField.text, (success) =>
            {
                if (success == true)
                    _inputTextField.text = SavedMessage;
            });
        }

        private void OnLoadButtonClicked()
        {
            PLink.Storage.Load(_inputKeyField.text, (success, data) =>
            {
                if (success == true)
                    _inputTextField.text = data;
            });
        }
    }
}