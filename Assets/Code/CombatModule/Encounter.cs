namespace ThroughAThousandEyes.CombatModule
{
    public abstract class Encounter
    {
        /// <summary>
        /// -1 means an endless encounter
        /// </summary>
        public abstract int LastWaveNumber { get; }

        public abstract Wave GetWave(int waveNumber);
        
        public virtual void OnWin() {}
        
        public virtual void OnLose() {}
        
        /// <summary>
        /// Called after OnWin or OnLose
        /// </summary>
        public virtual void OnEnd() {}
    }
}