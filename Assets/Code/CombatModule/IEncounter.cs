namespace ThroughAThousandEyes.CombatModule
{
    public interface IEncounter
    {
        /// <summary>
        /// -1 means an endless encounter
        /// </summary>
        int LastWaveNumber { get; }

        Wave GetWave(int waveNumber);
    }
}