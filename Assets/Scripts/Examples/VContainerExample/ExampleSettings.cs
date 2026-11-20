namespace Unity.BossRoom.Examples.VContainerExample
{
    /// <summary>
    /// 纯 C# 配置对象：不继承 MonoBehaviour，无法在 Inspector 里拖拽赋值，
    /// 所以需要用 <c>builder.RegisterInstance</c> 交给容器管理。
    /// </summary>
    public sealed class ExampleSettings
    {
        public string PlayerName { get; }
        public int StartingScore { get; }
        public float SpawnInterval { get; }

        public ExampleSettings(string playerName, int startingScore, float spawnInterval)
        {
            PlayerName = playerName;
            StartingScore = startingScore;
            SpawnInterval = spawnInterval;
        }
    }
}
