using DG.Tweening;
using Logic;
using TMPro;
using UnityEngine;
using VContainer;

namespace UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TaskText : MonoBehaviour
    {
        private const float FadeDuration = 1f;

        private GameService _gameService;
        private TMP_Text _taskText;

        [Inject]
        public void Construct(GameService gameService)
        {
            _gameService = gameService;
        }
        
        private void Awake()
        {
            _taskText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _gameService.TaskChanged += SetTask;
            _gameService.GameRestarted += PlayAnimation;
        }

        private void OnDisable()
        {
            _gameService.TaskChanged -= SetTask;
            _gameService.GameRestarted -= PlayAnimation;
        }

        private void SetTask(string cardIdentifier)
        {
            _taskText.text = $"Find {cardIdentifier}";
        }

        private void PlayAnimation()
        {
            PlayFadeEffect(FadeDuration);
        }

        private void PlayFadeEffect(float duration)
        {
            _taskText.DOKill();
            _taskText.alpha = 0f;
            _taskText
                .DOFade(1f, duration)
                .SetLink(_taskText.gameObject);
        }
    }
}