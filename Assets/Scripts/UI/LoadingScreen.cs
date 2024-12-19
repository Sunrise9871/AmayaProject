using DG.Tweening;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        private const float FadeDuration = 1f;
        private const float FadeStrength = 0.5f;

        private GameService _gameService;

        [SerializeField] private Image _background;

        private Sequence _fadeSequence;

        [Inject]
        public void Construct(GameService gameService)
        {
            _gameService = gameService;
        }

        private void Awake()
        {
            ChangeBackgroundAlpha(1f);
            _fadeSequence = DOTween.Sequence();
        }

        private void OnEnable()
        {
            _gameService.GameLoadingStarted += PlayFadeInEffect;
            _gameService.GameRestarted += PlayFadeOutEffect;
        }

        private void OnDisable()
        {
            _gameService.GameLoadingStarted -= PlayFadeInEffect;
            _gameService.GameRestarted -= PlayFadeOutEffect;
        }


        private void PlayFadeEffect(float targetAlpha, bool deactivateOnComplete)
        {
            _fadeSequence?.Kill();

            ChangeBackgroundAlpha(targetAlpha == 0f ? 1f : 0f);

            _background.gameObject.SetActive(true);

            _fadeSequence = DOTween.Sequence();
            _fadeSequence
                .Append(_background.DOFade(targetAlpha, FadeDuration)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        if (deactivateOnComplete)
                        {
                            _background.gameObject.SetActive(false);
                        }
                    }));
        }

        private void PlayFadeOutEffect()
        {
            PlayFadeEffect(0f, true);
        }

        private void PlayFadeInEffect()
        {
            PlayFadeEffect(FadeStrength, false);
        }

        private void ChangeBackgroundAlpha(float alpha)
        {
            var currentColor = _background.color;
            currentColor.a = alpha;
            _background.color = currentColor;
        }
    }
}