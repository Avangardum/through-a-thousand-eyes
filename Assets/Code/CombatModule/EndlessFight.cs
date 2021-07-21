using System;

namespace ThroughAThousandEyes.CombatModule
{
    public class EndlessFight : IEncounter
    {
        private CombatModuleRoot _root;
        private EndlessFightData _data;

        public int LastWaveNumber => -1;
        
        public EndlessFight(CombatModuleRoot root, EndlessFightData data)
        {
            _root = root;
            _data = data;
        }
        
        /// <summary>
        /// Returns a wave by its number (starts from 1)
        /// </summary>
        /// <param name="waveNumber"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public Wave GetWave(int waveNumber)
        {
            if (waveNumber < 1)
            {
                throw new ArgumentException();
            }
            
            Wave wave = new Wave();
            wave.AddEnemy(new EndlessFightEnemy
            (
                root: _root,
                maxHp: _data.EnemyHp.GetElement(waveNumber),
                armor: _data.EnemyArmor.GetElement(waveNumber),
                damage: _data.EnemyDamage.GetElement(waveNumber),
                attackSpeed: _data.EnemyAttackSpeed.GetElement(waveNumber),
                expReward: _data.EnemyExpReward.GetElement(waveNumber)
            ));

            return wave;
        }
    }
}