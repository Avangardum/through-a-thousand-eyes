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
                    return (dynamic)InitialTerm + ((dynamic)termNumber - 1) * (dynamic)CommonRatioOrDifference;
                    break;
                case ProgressionTypeEnum.Geometric:
                    if (termNumber == 1)
                    {
                        return InitialTerm;
                    }
                    else
                    {
                        return (dynamic)GetElement(termNumber - 1) * (dynamic)CommonRatioOrDifference;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}