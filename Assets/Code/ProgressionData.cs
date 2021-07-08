using System;
using UnityEngine;

namespace ThroughAThousandEyes
{
    /// <summary>
    /// Stores data of an arithmetic or geometric progression
    /// </summary>
    /// <typeparam name="T">Requirement for the T type : T * T and T + T operations should be defined</typeparam>
    [Serializable]
    public class ProgressionData<T>
    {
        public enum ProgressionTypeEnum
        {
            Arithmetic = 0,
            Geometric = 1
        }

        [field: SerializeField] public ProgressionTypeEnum ProgressionType { get; private set; }
        [field: SerializeField] public T InitialTerm { get; private set; }
        [field: SerializeField] public T CommonRatioOrDifference { get; private set; }

        private static Func<T, T, T> _add;
        private static Func<T, T, T> _multiply;

        public static void Initialize(Func<T, T, T> add, Func<T, T, T> multiply)
        {
            _add = add;
            _multiply = multiply;
        }
        
        static ProgressionData()
        {
            ProgressionDataInitializer.Initialize();
        }
        
        /// <summary>
        /// Returns the progression term by its number (numbers start with 1)
        /// </summary>
        /// <param name="termNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T GetElement(long termNumber)
        {
            if (termNumber <= 0)
            {
                throw new ArgumentException();
            }
            
            switch (ProgressionType)
            {
                case ProgressionTypeEnum.Arithmetic:
                    if (termNumber == 1)
                    {
                        return InitialTerm;
                    }
                    else
                    {
                        return _add(GetElement(termNumber - 1), CommonRatioOrDifference);
                    }
                case ProgressionTypeEnum.Geometric:
                    if (termNumber == 1)
                    {
                        return InitialTerm;
                    }
                    else
                    {
                        return _multiply(GetElement(termNumber - 1), CommonRatioOrDifference);
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}