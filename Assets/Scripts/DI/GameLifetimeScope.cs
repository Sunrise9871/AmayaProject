using Grid;
using Logic;
using UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameService _gameService;
        [SerializeField] private RestartScreen _restartScreen;
        [SerializeField] private GameGrid _gameGrid;
        [SerializeField] private TaskText _taskText;
        [SerializeField] private LoadingScreen _loadingScreen;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(_gameService).As<GameService>();
            builder.RegisterComponent(_restartScreen).As<RestartScreen>();
            builder.RegisterComponent(_gameGrid).As<GameGrid>();
            builder.RegisterComponent(_taskText).As<TaskText>();
            builder.RegisterComponent(_loadingScreen).As<LoadingScreen>();
        }
    }
}