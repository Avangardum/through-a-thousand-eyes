using System;
using System.Collections.Generic;

namespace ThroughAThousandEyes.CombatModule
{
    public class KingdomAttack : Encounter
    {
        private KingdomAttackData _data;
        private CombatModuleRoot _root;

        public KingdomAttack(KingdomAttackData data, CombatModuleRoot root)
        {
            _data = data;
            _root = root;
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
                    units.Add(new KingdomWarrior(_root, _data.KingdomWarriorStats, Side.Enemies));
                }

                return new Wave(units);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}