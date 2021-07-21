namespace ThroughAThousandEyes.CombatModule
{
    public class StressTestEncounter : IEncounter
    {
        private const int Waves = 1;
        private const int AllyCount = 105;
        private const int EnemyCount = 100;

        private CombatModuleRoot _root;
        
        public int LastWaveNumber => Waves; 
        public Wave GetWave(int waveNumber)
        {
            var wave = new Wave();
            for (int i = 0; i < EnemyCount; i++)
            {
                wave.AddEnemy(new TestUnit(
                    root: _root,
                    maxHp: 100,
                    armor: 1,
                    damage: 10,
                    attackSpeed: 1,
                    side: Side.Enemies
                    ));
            }

            return wave;
        }

        public StressTestEncounter(CombatModuleRoot root)
        {
            _root = root;
        }
    }
}