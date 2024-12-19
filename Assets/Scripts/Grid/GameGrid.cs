using System.Collections.Generic;
using System.Threading.Tasks;
using Cards;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Grid
{
    public class GameGrid : MonoBehaviour
    {
        private readonly List<CardView> _cells = new List<CardView>();

        [SerializeField] private CardView _cellPrefab;
        [SerializeField] private Vector2 _spacing;

        private Vector2 _cellSize;

        private IObjectResolver _objectResolver;

        [Inject]
        public void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        private void Awake()
        {
            var cellSize = _cellPrefab.GetComponent<RectTransform>().rect;
            _cellSize = new Vector2(cellSize.width, cellSize.height);
        }

        public async Task<List<CardView>> GenerateNewGrid(int rows, int columns)
        {
            return await ConstructGrid(rows, columns, true);
        }

        public async Task<List<CardView>> UpdateGrid(int rows, int columns)
        {
            return await ConstructGrid(rows, columns, false);
        }

        private async Task<List<CardView>> ConstructGrid(int rows, int columns, bool clearExisting)
        {
            if (clearExisting)
            {
                ClearGrid();
            }

            var totalCellsNeeded = rows * columns;
            while (_cells.Count < totalCellsNeeded)
            {
                AddCell();
            }
            while (_cells.Count > totalCellsNeeded)
            {
                RemoveCell();
            }

            for (var row = 0; row < rows; row++)
            {
                for (var column = 0; column < columns; column++)
                {
                    var index = row * columns + column;
                    var position = CalculateCellPosition(row, column);
                    _cells[index].transform.localPosition = position;
                    
                    await Task.Yield();
                }
            }

            CenterGrid(rows, columns);
            
            return _cells;
        }

        private void AddCell()
        {
            var cell = _objectResolver.Instantiate(_cellPrefab);
            cell.transform.SetParent(transform, false);
            _cells.Add(cell);
        }

        private void RemoveCell()
        {
            var cellToRemove = _cells[_cells.Count - 1];
            _cells.RemoveAt(_cells.Count - 1);
            Destroy(cellToRemove.gameObject);
        }

        private Vector3 CalculateCellPosition(int row, int column)
        {
            var x = column * (_cellSize.x + _spacing.x);
            var y = -row * (_cellSize.y + _spacing.y);
            return new Vector3(x, y, 0);
        }

        private void CenterGrid(int rows, int columns)
        {
            var gridWidth = columns * _cellSize.x + (columns - 1) * _spacing.x - _cellSize.x;
            var gridHeight = rows * _cellSize.y + (rows - 1) * _spacing.y - _cellSize.y;

            var gridCenter = new Vector3(-gridWidth / 2, gridHeight / 2, 0);
            transform.localPosition = gridCenter;
        }

        private void ClearGrid()
        {
            foreach (var cell in _cells)
            {
                Destroy(cell.gameObject);
            }

            _cells.Clear();
        }
    }
}