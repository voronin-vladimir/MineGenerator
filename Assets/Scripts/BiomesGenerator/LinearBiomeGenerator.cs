using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace BiomesGenerator
{
    public class LinearBiomeGenerator
    {
        private readonly struct Chunk
        {
            public readonly Vector2Int LeftBottomCorner;
            public readonly Vector2Int RightTopCorner;
            public readonly int Biome;
            public readonly HashSet<Vector2Int> GrowPoints;

            public Chunk(Vector2Int leftBottomCorner, Vector2Int rightTopCorner, int biome,
                Vector2Int originPoint) : this()
            {
                LeftBottomCorner = leftBottomCorner;
                RightTopCorner = rightTopCorner;
                Biome = biome;
                GrowPoints = new HashSet<Vector2Int> { originPoint };
            }
        }

        private const int ChunkSize = 8;
        private System.Random _rnd;

        public int[,] Generate(int width, int height)
        {
            var map = new int[width, height];
            _rnd = new System.Random();
            var chunks = new List<Chunk>();

            for (var x = 0; x < width; x += ChunkSize)
            {
                for (var y = 0; y < height; y += ChunkSize)
                {
                    var biomeInChunkCount = _rnd.Next(1, 3);

                    for (var i = 0; i < biomeInChunkCount; i++)
                    {
                        var leftBottomCorner = new Vector2Int(x, y);
                        var rightTopCorner = new Vector2Int(x + 8, y + 8);
                        var biome = PickRandomBiome();
                        var originPoint = PickRandomPositionInChunk(leftBottomCorner, rightTopCorner);
                        var chunk = new Chunk(leftBottomCorner, rightTopCorner, biome, originPoint);

                        chunks.Add(chunk);
                        map[originPoint.x, originPoint.y] = chunk.Biome;
                    }
                }
            }

            var isFinished = false;

            while (!isFinished)
            {
                foreach (var chunk in chunks)
                {
                    if (!chunk.GrowPoints.Any())
                    {
                        isFinished = true;
                        continue;
                    }

                    isFinished = false;

                    var growPoints = chunk.GrowPoints.ToArray();

                    foreach (var point in growPoints)
                    {
                        chunk.GrowPoints.Remove(point);
                        var neighbours = CellsExtensions.GetNeighbours(point, chunk.LeftBottomCorner,
                            chunk.RightTopCorner).Where(n => map[n.x, n.y] == 0).ToArray();

                        if (!neighbours.Any())
                        {
                            continue;
                        }

                        foreach (var pos in neighbours)
                        {
                            var isGrowing = _rnd.Next(0, 100) < 50;

                            if (!isGrowing)
                                continue;

                            map[pos.x, pos.y] = chunk.Biome;
                            chunk.GrowPoints.Add(pos);
                        }
                    }
                }
            }

            return map;
        }

        private int PickRandomBiome()
        {
            return _rnd.Next(1, 4);
        }

        private Vector2Int PickRandomPositionInChunk(Vector2Int leftBottom, Vector2Int rightTop)
        {
            var x = _rnd.Next(leftBottom.x, rightTop.x);
            var y = _rnd.Next(leftBottom.y, rightTop.y);
            return new Vector2Int(x, y);
        }
    }
}