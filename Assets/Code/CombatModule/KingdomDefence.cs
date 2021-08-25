using System;
using System.Linq;

namespace ThroughAThousandEyes.CombatModule
{
    public class KingdomDefence : IEncounter
    {
        private readonly CombatModuleRoot _root;
        private readonly KingdomDefenceData _data;
        private readonly int _stage;

        public int LastWaveNumber => 1;
        
        public Wave GetWave(int waveNumber)
        {
            if (waveNumber != 1)
                throw new ArgumentException("Wrong wave number");

            return new Wave(Enumerable.Range(1, _data.EnemyAmount.GetElement(_stage))
                .Select(x => new KingdomWarrior(_root, _data.KingdomWarriorStats, Side.Enemies)).ToList());
        }

        public KingdomDefence(CombatModuleRoot root, KingdomDefenceData data, int stage)
        {
            _root = root;
            _data = data;
            _stage = stage;
        }
    }
}