using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public class WeightedList<T>
    {
        private readonly List<T> _items = new();
        private readonly List<int> _weights = new();
        private readonly Random _rnd;

        private int _totalWeight;
        
        public WeightedList()
        {
            _rnd = new Random();
        }

        public void Add(T item, int weight)
        {
            if (weight <= 0)
                throw new ArgumentException("Weight must be > 0");

            _items.Add(item);
            _weights.Add(weight);
            _totalWeight += weight;
        }

        public T GetRandom()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("WeightedList пуст");

            var rand = _rnd.Next(_totalWeight);

            for (var i = 0; i < _items.Count; i++)
            {
                if (rand < _weights[i])
                    return _items[i];

                rand -= _weights[i];
            }

            return _items.Last();
        }

        public void Clear()
        {
            _items.Clear();
            _weights.Clear();
            _totalWeight = 0;
        }
    }
}