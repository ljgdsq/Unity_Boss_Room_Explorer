using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer.Unity;

namespace Unity.BossRoom.Examples.VContainerExample
{
    /// <summary>
    /// EntryPoint：普通 C# 类也能拿到 Start / Tick / Dispose 回调，不需要 MonoBehaviour。
    /// 构造函数里的依赖全部由 VContainer 注入，包括 <c>Func&lt;Bot&gt;</c> 这个工厂。
    /// </summary>
    public sealed class ExampleGameLoop : IStartable, ITickable, IDisposable
    {
        readonly IScoreService m_ScoreService;
        readonly ExampleHud m_Hud;
        readonly Func<Bot> m_CreateBot;
        readonly ExampleSettings m_Settings;
        readonly List<Bot> m_Bots = new List<Bot>();

        float m_Elapsed;
        float m_NextSpawn;
        float m_NextScore;

        public ExampleGameLoop(
            IScoreService scoreService,
            ExampleHud hud,
            Func<Bot> createBot,
            ExampleSettings settings)
        {
            m_ScoreService = scoreService;
            m_Hud = hud;
            m_CreateBot = createBot;
            m_Settings = settings;
        }

        /// <summary>容器构建完成后的第一帧调用（PlayerLoop 的 Startup 阶段）。</summary>
        public void Start()
        {
            m_Hud.ShowMessage($"玩家 {m_Settings.PlayerName}：每 {m_Settings.SpawnInterval:0.#} 秒生成一个 Bot");
            Debug.Log($"[VContainer] ExampleGameLoop.Start(): 初始分数 = {m_ScoreService.Score}");
        }

        /// <summary>每帧调用（PlayerLoop 的 Update 阶段）。</summary>
        public void Tick()
        {
            m_Elapsed += Time.deltaTime;

            if (m_Elapsed >= m_NextSpawn)
            {
                m_NextSpawn = m_Elapsed + m_Settings.SpawnInterval;
                m_Bots.Add(m_CreateBot());
            }

            for (var i = 0; i < m_Bots.Count; i++)
            {
                m_Bots[i].Tick(Time.deltaTime);
            }

            if (m_Elapsed >= m_NextScore)
            {
                m_NextScore = m_Elapsed + 1f;
                m_ScoreService.Add(1);
            }
        }

        /// <summary>容器 Dispose 时调用（离开场景 / 停止 Play）。</summary>
        public void Dispose()
        {
            foreach (var bot in m_Bots)
            {
                bot.Dispose();
            }

            m_Bots.Clear();
            Debug.Log("[VContainer] ExampleGameLoop.Dispose(): Scope 已释放");
        }
    }
}
