using DG.Tweening;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Cards
{
    public class CardView : MonoBehaviour
    {
        private const float ShakeStrength = 20f;
        private const float ShakeDuration = 0.5f;

        private const float BounceDuration = 1.5f;
        private const float FirstKey = 0f;
        private const float SecondKey = 1.2f;
        private const float ThirdKey = 0.95f;

        [SerializeField] private Image _taskImage;
        [SerializeField] private ParticleSystem _particleSystem;

        private GameService _gameService;

        private Vector3 _originalPosition;
        private Vector3 _originalScale;

        public CardData CardData { get; private set; }

        [Inject]
        public void Construct(GameService gameService)
        {
            _gameService = gameService;
        }

        private void Start()
        {
            _originalPosition = _taskImage.transform.localPosition;
            _originalScale = _taskImage.transform.localScale;
        }

        private void OnEnable()
        {
            _gameService.GameRestarted += PlayAppearanceEffect;
        }

        private void OnDisable()
        {
            _gameService.GameRestarted -= PlayAppearanceEffect;
        }

        public void InitializeCard(CardData cardData)
        {
            CardData = cardData;
            _taskImage.sprite = cardData.Sprite;
            _taskImage.transform.rotation = Quaternion.Euler(0f, 0f, -cardData.SpriteRotatedByDegrees);
        }

        public void OnPointerDown()
        {
            _gameService.SelectCard(this);
        }

        public void PlayWrongAnswerEffect()
        {
            _taskImage.transform.DOKill();
            _taskImage.transform.localPosition = _originalPosition;

            _taskImage.transform
                .DOShakePosition(ShakeDuration, new Vector3(ShakeStrength, 0f, 0f))
                .SetEase(Ease.InBounce)
                .SetLink(transform.gameObject)
                .OnComplete(() =>
                {
                    _taskImage.transform.localPosition = _originalPosition;
                });
        }

        public void PlayCorrectAnswerEffect()
        {
            PlayBounceEffect(_taskImage.transform).OnKill(_gameService.PickNextLevel);

            _particleSystem.Play();
        }

        private void PlayAppearanceEffect()
        {
            PlayBounceEffect(transform);
        }

        private Sequence PlayBounceEffect(Transform objectTransform)
        {
            objectTransform.DOKill();
            objectTransform.localScale = _originalScale;

            var bounceSequence = DOTween.Sequence();
            bounceSequence.Append(objectTransform.DOScale(_originalScale * FirstKey, BounceDuration / 4));
            bounceSequence.Append(objectTransform.DOScale(_originalScale * SecondKey, BounceDuration / 4));
            bounceSequence.Append(objectTransform.DOScale(_originalScale * ThirdKey, BounceDuration / 4));
            bounceSequence.Append(objectTransform.DOScale(_originalScale, BounceDuration / 4));

            return bounceSequence
                .SetLink(objectTransform.gameObject)
                .Play();
        }
    }
}