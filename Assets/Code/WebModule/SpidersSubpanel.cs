using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ThroughAThousandEyes.WebModule
{
    public class SpidersSubpanel : MonoBehaviour
    {
        public Spider Spider
        {
            get => _spider;
            set
            {
                _spider = value;
                switch (_spider.State)
                {
                    case Spider.StateEnum.Weaving:
                        OnWeaveClick();
                        break;
                    case Spider.StateEnum.Feeding:
                        OnFeedClick();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        [SerializeField] private Button _feedButton;
        [SerializeField] private Button _weaveButton;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Color _pressedButtonColor = new Color(0.8f, 0.8f, 0.8f);
        [SerializeField] private Color _normalButtonColor = Color.white;
        
        private Spider _spider;
        private Image _feedButtonImage;
        private Image _weaveButtonImage;

        public string Name
        {
            set => _nameText.text = value;
        }

        public long Level
        {
            set => _levelText.text = $"Level {value}";
        }

        private void Awake()
        {
            _feedButtonImage = _feedButton.GetComponent<Image>();
            _weaveButtonImage = _weaveButton.GetComponent<Image>();
        }

        private void OnEnable()
        {
            _feedButton.onClick.AddListener(OnFeedClick);
            _weaveButton.onClick.AddListener(OnWeaveClick);
        }

        private void OnDisable()
        {
            _feedButton.onClick.RemoveAllListeners();
            _weaveButton.onClick.RemoveAllListeners();
        }

        private void OnFeedClick()
        {
            Spider.State = Spider.StateEnum.Feeding;
            _feedButtonImage.color = _pressedButtonColor;
            _weaveButtonImage.color = _normalButtonColor;
        }

        private void OnWeaveClick()
        {
            Spider.State = Spider.StateEnum.Weaving;
            _feedButtonImage.color = _normalButtonColor;
            _weaveButtonImage.color = _pressedButtonColor;
        }
    }
}