using System;
using System.Linq;
using MazeGenerator;
using NodeGenerator;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MazeCellView : MonoBehaviour
    {
        [Serializable]
        private struct Tile
        {
            public WallPosition Position;
            public Sprite Sprite;
        }

        [SerializeField] private Tile[] _walls;
        [SerializeField] private Image _cellImage;
        [SerializeField] private Image _biomeImage;
        [SerializeField] private Color[] _biomeColors;
        
        [SerializeField] private GameObject _nodeMarker;

        private Node _node;
        
        public void SetInfo(MazeCell cellData, int biome)
        {
            var sprite = _walls.First(w => w.Position == cellData.Walls).Sprite;
            _cellImage.sprite = sprite;
            _biomeImage.color = _biomeColors[biome];
        }

        public void SetNode(Node node)
        {
            _node = node;
            _nodeMarker.SetActive(true);
        }
    }
}