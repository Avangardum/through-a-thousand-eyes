using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughAThousandEyes.WebModule
{
    public class SpidersSubpanel : MonoBehaviour
    {
        public event Action FeedClicked;
        public event Action WeaveClicked;

        [SerializeField] private Button _feedButton;
        [SerializeField] private Button _weaveButton;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        public string Name
        {
            set => _nameText.text = value;
        }

        public long Level
        {
            set => _levelText.text = $"Level {value}";
        }

        private void OnEnable()
        {
            _feedButton.onClick.AddListener(() => FeedClicked?.Invoke());
            _weaveButton.onClick.AddListener(() => WeaveClicked?.Invoke());
        }

        private void OnDisable()
        {
            _feedButton.onClick.RemoveAllListeners();
            _weaveButton.onClick.RemoveAllListeners();
        }
    }
}