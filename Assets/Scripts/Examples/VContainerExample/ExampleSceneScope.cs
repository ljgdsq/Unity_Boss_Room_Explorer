using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity.BossRoom.Examples.VContainerExample
{
    /// <summary>
    /// 场景级 LifetimeScope：一个场景一个容器。
    /// 它和 Boss Room 里的 <c>ApplicationController</c> / <c>GameStateBehaviour</c> 是同一个角色。
    /// </summary>
    public sealed class ExampleSceneScope : LifetimeScope
    {
        [SerializeField]
        ExampleHud m_Hud;

        [SerializeField]
        string m_PlayerName = "VContainer 玩家";

        [SerializeField]
        int m_StartingScore;

        [SerializeField]
        float m_SpawnInterval = 2f;

        static int s_BotCounter;

        protected override void Configure(IContainerBuilder builder)
        {
            // 1) 纯 C# 对象：注册一个现成实例（Lifetime 由你控制，容器不 new 它）
            builder.RegisterInstance(new ExampleSettings(m_PlayerName, m_StartingScore, m_SpawnInterval));

            // 2) 接口 → 实现 + 生命周期。AsSelf() 表示同时可以用具体类型解析
            builder.Register<ScoreService>(Lifetime.Singleton)
                .As<IScoreService>()
                .AsSelf();

            // 3) 工厂：注册 Func<Bot>，每次调用产生一个新对象
            builder.RegisterFactory<Bot>(CreateBot);

            // 4) 场景里的 MonoBehaviour：交给容器托管，构建容器时立即执行 [Inject]
            var hud = m_Hud != null ? m_Hud : GetComponent<ExampleHud>();
            if (hud == null)
            {
                hud = gameObject.AddComponent<ExampleHud>();
            }

            m_Hud = hud;
            builder.RegisterComponent(hud);

            // 5) EntryPoint：让普通类获得 Start / Tick / Dispose 回调
            builder.RegisterEntryPoint<ExampleGameLoop>();

            // 6) 容器构建完成后的回调：适合做日志、校验、初始化
            builder.RegisterBuildCallback(container =>
            {
                var settings = container.Resolve<ExampleSettings>();
                Debug.Log($"[VContainer] 容器构建完成，玩家 = {settings.PlayerName}");
            });
        }

        // 工厂方法：由容器在 Resolve<Func<Bot>> 时调用
        Bot CreateBot()
        {
            var botObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            botObject.name = $"Bot-{++s_BotCounter}";
            botObject.transform.SetParent(transform, false);

            var angle = Random.Range(0f, Mathf.PI * 2f);
            var radius = Random.Range(2.5f, 4.5f);
            botObject.transform.localPosition = new Vector3(Mathf.Cos(angle) * radius, 0.5f, Mathf.Sin(angle) * radius);
            botObject.transform.localScale = Vector3.one * 0.6f;
            botObject.GetComponent<Renderer>().material.color =
                Random.ColorHSV(0f, 1f, 0.65f, 1f, 0.85f, 1f);

            return new Bot(botObject.transform, Random.Range(30f, 120f));
        }
    }
}
