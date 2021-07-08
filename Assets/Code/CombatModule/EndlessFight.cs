using System;

namespace ThroughAThousandEyes.CombatModule
{
    public class EndlessFight : IEncounter
    {
        private EndlessFightData _data;
        
        public EndlessFight(EndlessFightData data)
        {
            _data = data;
        }
        
        // Returns a wave by its number (starts from 1)
        public Wave GetWave(int waveNumber)
        {
            if (waveNumber < 1)
            {
                throw new ArgumentException();
            }
            
            Wave wave = new Wave();
            wave.AddEnemy(new Unit
            {
                MaxHp = _data.EnemyHp.GetElement(waveNumber),
                CurrentHp = _data.EnemyHp.GetElement(waveNumber),
                Armor = _data.EnemyArmor.GetElement(waveNumber),
                Damage = _data.EnemyDamage.GetElement(waveNumber),
                AttackSpeed = _data.EnemyAttackSpeed.GetElement(waveNumber)
            });

            return wave;
        }
    }
}