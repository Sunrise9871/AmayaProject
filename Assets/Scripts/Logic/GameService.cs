using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cards;
using Grid;
using Levels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic
{
    public class GameService : MonoBehaviour
    {
        private readonly List<CardData> _taskTargets = new List<CardData>();
        
        [SerializeField] private CardBundleData[] _cardBundleData;
        [SerializeField] private LevelsSetData _levelsSetData;
        [SerializeField] private GameGrid _gameGrid;

        private int _nextLevel;
        private bool _controlBlock;

        public event Action<string> TaskChanged;
        public event Action GameRestarted;
        public event Action GameFinished;
        public event Action GameLoadingStarted; 

        private async void Start()
        {
            await RestartGame();
        }

        public async Task RestartGame()
        {
            GameLoadingStarted?.Invoke();
            
            _nextLevel = 0;
            _controlBlock = false;
            
            var levelData = _levelsSetData.LevelsData[_nextLevel++];
            var cells = await _gameGrid.GenerateNewGrid(levelData.RowsCount, levelData.ColumnsCount);
            
            await LoadLevel(cells);
            
            GameRestarted?.Invoke();
        }
        
        public void SelectCard(CardView cardView)
        {
            if (_controlBlock)
                return;
            
            var taskTarget = _taskTargets[_taskTargets.Count - 1];

            if (cardView.CardData == taskTarget)
            {
                cardView.PlayCorrectAnswerEffect();
                
                _controlBlock = true;
            }
            else
            {
                cardView.PlayWrongAnswerEffect();
            }
        }
        
        public async void PickNextLevel()
        {
            if (_nextLevel < _levelsSetData.LevelsData.Length)
            {
                var levelData = _levelsSetData.LevelsData[_nextLevel++];
                var cells = await _gameGrid.UpdateGrid(levelData.RowsCount, levelData.ColumnsCount);
                
                await LoadLevel(cells);

                _controlBlock = false;
            }
            else
            {
                GameFinished?.Invoke();
            }
        }

        private async Task LoadLevel(List<CardView> cells)
        {
            var cardBundleIndex = Random.Range(0, _cardBundleData.Length);
            var taskTargetCard = PickTaskTargetCard(cardBundleIndex);

            var availableCards = new List<CardData>(_cardBundleData[cardBundleIndex].CardData);
            availableCards.Remove(_taskTargets[_taskTargets.Count - 1]);
            
            var targetCardCellIndex = Random.Range(0, cells.Count);
            for (var i = 0; i < cells.Count; i++)
            {
                if (i == targetCardCellIndex)
                {
                    cells[i].InitializeCard(taskTargetCard);

                    TaskChanged?.Invoke(taskTargetCard.Identifier);
                    continue;
                }

                await Task.Yield();
                
                var wrongCardIndex = Random.Range(0, availableCards.Count);
                var wrongCard = availableCards[wrongCardIndex];
                availableCards.Remove(wrongCard);

                cells[i].InitializeCard(wrongCard);
            }
        }

        private CardData PickTaskTargetCard(int cardBundleIndex)
        {
            var availableCards = new List<CardData>(_cardBundleData[cardBundleIndex].CardData);
            foreach (var taskTarget in _taskTargets)
            {
                availableCards.Remove(taskTarget);
            }

            var taskTargetIndex = Random.Range(0, availableCards.Count);
            var taskTargetCard = availableCards[taskTargetIndex];
            _taskTargets.Add(taskTargetCard);

            return taskTargetCard;
        }
    }
}