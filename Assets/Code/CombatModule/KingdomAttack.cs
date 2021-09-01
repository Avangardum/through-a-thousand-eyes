using System;
using System.Collections.Generic;

namespace ThroughAThousandEyes.CombatModule
{
    public class KingdomAttack : Encounter
    {
        private readonly KingdomAttackData _data;
        private readonly CombatModuleRoot _root;
        private readonly int _defenceStagesPassed;

        public KingdomAttack(KingdomAttackData data, CombatModuleRoot root, int defenceStagesPassed)
        {
            _data = data;
            _root = root;
            _defenceStagesPassed = defenceStagesPassed;
        }

        public override int LastWaveNumber => _data.CommonWavesAmount + 1;
        
        public override Wave GetWave(int waveNumber)
        {
            if (waveNumber < 1 || waveNumber > LastWaveNumber)
                throw new ArgumentOutOfRangeException();

            if (waveNumber <= _data.CommonWavesAmount)
            {
                var units = new List<Unit>();
                for (int i = 0; i < _data.KingdomWarriorsInCommonWaveByWaveNumber.GetElement(waveNumber); i++)
                {
                    units.Add(new KingdomWarrior
                        (_root, _data.KingdomWarriorStatsAfterDecrease(_defenceStagesPassed), Side.Enemies));
                }

                return new Wave(units);
            }
            else
            {
                return new Wave(new King(_root, _data.KingStatsAfterDecrease(_defenceStagesPassed),
                    _data.KnightStatsAfterDecrease(_defenceStagesPassed)));
            }
        }
    }
}