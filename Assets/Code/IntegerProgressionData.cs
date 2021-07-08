using System;
using UnityEngine;

namespace ThroughAThousandEyes
{
    [Serializable]
    [Obsolete]
    public class IntegerProgressionData
    {
        public enum ProgressionTypeEnum
        {
            Arithmetic = 0,
            Geometric = 1
        }

        [field: SerializeField] public ProgressionTypeEnum ProgressionType { get; private set; }
        [field: SerializeField] public long InitialTerm { get; private set; }
        [field: SerializeField] public long CommonRatioOrDifference { get; private set; }

        /// <summary>
        /// Returns the progression term by its number (numbers start with 1)
        /// </summary>
        /// <param name="termNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public long GetElement(long termNumber)
        {
            switch (ProgressionType)
            {
                case ProgressionTypeEnum.Arithmetic:
                    return InitialTerm + (termNumber - 1) * CommonRatioOrDifference;
                    break;
                case ProgressionTypeEnum.Geometric:
                    return InitialTerm * (long)Math.Pow(CommonRatioOrDifference, termNumber - 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}