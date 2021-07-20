using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThroughAThousandEyes.CombatModule
{
    [Serializable]
    public class ArrayWithOccupiedFlags<T> : IEnumerable<T>
    {
        public int Length { get; private set; }
        [SerializeField] private T[] _array;
        private bool[] _occupiedFlags;

        public T this[int i]
        {
            get => _array[i];
            set => _array[i] = value;
        }

        public bool IsElementOccupied(int i) => _occupiedFlags[i];
        public void MarkSlotAsFree(int i) => _occupiedFlags[i] = false;
        public void MarkSlotAsOccupied(int i) => _occupiedFlags[i] = true;

        public void MarkAllSlotsAsFree()
        {
            for (int i = 0; i < _occupiedFlags.Length; i++)
            {
                _occupiedFlags[i] = false;
            }
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            return (IEnumerator<T>)_array.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _array.GetEnumerator();
        }
        
        public ArrayWithOccupiedFlags(int length)
        {
            Length = length;
            _array = new T[length];
            _occupiedFlags = new bool[length];
        }

        public int GetFirstFreeIndex()
        {
            for (int i = 0; i < Length; i++)
            {
                if (!IsElementOccupied(i))
                {
                    return i;
                } 
            }

            return -1;
        }

        public void InitializeSerialized()
        {
            Length = _array.Length;
            _occupiedFlags = new bool[Length];
        }
    }
}