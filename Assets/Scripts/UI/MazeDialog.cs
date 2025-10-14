using System.Collections.Generic;
using BiomesGenerator;
using MazeGenerator;
using NodeGenerator;
using UnityEngine;

namespace UI
{
    public class MazeDialog : MonoBehaviour
    {
        [SerializeField] private int _mazeWidth;
        [SerializeField] private int _mazeDepth;

        [SerializeField] private MineMapView _mineMapView;

        private MazeCellView[,] _gridView;

        public void Start()
        {
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
    }
}