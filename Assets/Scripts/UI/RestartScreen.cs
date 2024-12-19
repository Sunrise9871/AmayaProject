using DG.Tweening;
using Logic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI
{
    public class RestartScreen : MonoBehaviour
    {
        private const float FadeDuration = 1f;
        private const float FadeStrength = 0.5f;

        [SerializeField] private Button _button;
        [SerializeField] private Image _background;
        
        private GameService _gameService;

        [Inject]
        public void Construct(GameService gameService)
        {
            _gameService = gameService;
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(RestartGame);

            _gameService.GameFinished += ActivateRestartScreen;
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(RestartGame);
            
            _gameService.GameFinished -= ActivateRestartScreen;
        }

        private async void RestartGame()
        {
            await _gameService.RestartGame();
            
            _background.gameObject.SetActive(false);
        }

        private void PlayFadeInEffect()
        {
            _background.DOKill();

            _background
                .DOFade(FadeStrength, FadeDuration)
                .SetLink(_background.gameObject);
        }

        private void ActivateRestartScreen()
        {
            _background.gameObject.SetActive(true);
            PlayFadeInEffect();
        }
    }
}