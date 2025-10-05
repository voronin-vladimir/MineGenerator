using System.Collections.Generic;
using BiomesGenerator;
using MazeGenerator;
using NodeGenerator;
using UnityEngine;

namespace UI
{
    public class MazeDialog : MonoBehaviour
    {
        // [SerializeField] private MazeCellView _prefab;
        // [SerializeField] private Transform _container;

        [SerializeField] private int _mazeWidth;
        [SerializeField] private int _mazeDepth;

        [SerializeField] private MineMapView _mineMapView;

        private MazeCellView[,] _gridView;

        public void Start()
        {
            var mazeGenerator = new MazeGenerator.MazeGenerator();
            var maze = mazeGenerator.Generate(_mazeWidth, _mazeDepth);

            var biomeGenerator = new LinearBiomeGenerator();
            var biomes = biomeGenerator.Generate(_mazeWidth, _mazeDepth);

            var nodesGenerator = new NodesGenerator();
            var nodes = nodesGenerator.Generate(_mazeWidth, _mazeDepth);
            
            SetupView(biomes, nodes);
        }

        private void SetupView(int[,] biomes, List<Node> nodes)
        {
            _mineMapView.SetupBiomes(biomes);
            _mineMapView.SetupNodes(nodes);
        }

        // private void SetupView()
        // {
        // }
        //
        // private void Visualize(MazeCell[,] grid, int[,] biomes)
        // {
        //     _gridView = new MazeCellView[_mazeWidth, _mazeDepth];
        //
        //     for (var x = 0; x < _mazeWidth; x++)
        //     {
        //         for (var y = 0; y < _mazeDepth; y++)
        //         {
        //             var view = Instantiate(_prefab, _container);
        //
        //             _gridView[x, y] = view;
        //             view.SetInfo(grid[x, y], biomes[x, y]);
        //         }
        //     }
        // }
        //
        // private void SetupNodes(List<Node> nodes)
        // {
        //     foreach (var node in nodes)
        //     {
        //         SetupNode(node);
        //     }
        // }
        //
        // private void SetupNode(Node node)
        // {
        //     var nodeView = _gridView[node.Position.x, node.Position.y];
        //     nodeView.SetNode(node);
        // }
    }
}