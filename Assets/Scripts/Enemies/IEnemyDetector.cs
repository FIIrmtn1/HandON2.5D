namespace HandOnTheLine.Enemies
{
    /// <summary>Common contract for enemy detection strategies (sound, hearing, proximity, vision).</summary>
    public interface IEnemyDetector
    {
        /// <summary>0.0 (unaware) to 1.0 (fully alerted).</summary>
        float SuspicionLevel { get; }
    }
}
