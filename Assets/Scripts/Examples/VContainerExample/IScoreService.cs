using System;

namespace Unity.BossRoom.Examples.VContainerExample
{
    /// <summary>
    /// 服务接口：注册时把实现绑定到这个接口，调用方只依赖接口。
    /// </summary>
    public interface IScoreService
    {
        int Score { get; }

        event Action<int> ScoreChanged;

        void Add(int amount);
    }
}
