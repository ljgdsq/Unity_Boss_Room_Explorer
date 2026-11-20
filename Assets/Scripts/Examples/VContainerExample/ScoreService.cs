using System;

namespace Unity.BossRoom.Examples.VContainerExample
{
    /// <summary>
    /// 服务的实现：构造函数注入 <see cref="ExampleSettings"/>，
    /// VContainer 在第一次 Resolve 时会自动把已注册的实例传进来。
    /// </summary>
    public sealed class ScoreService : IScoreService
    {
        readonly ExampleSettings m_Settings;

        public int Score { get; private set; }

        public event Action<int> ScoreChanged;

        public ScoreService(ExampleSettings settings)
        {
            m_Settings = settings;
            Score = settings.StartingScore;
        }

        public void Add(int amount)
        {
            if (amount == 0)
            {
                return;
            }

            Score += amount;
            ScoreChanged?.Invoke(Score);
        }

        public override string ToString() => $"{m_Settings.PlayerName}: {Score}";
    }
}
