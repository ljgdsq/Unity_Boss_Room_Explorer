using UnityEngine;
using VContainer;

namespace Unity.BossRoom.Examples.VContainerExample
{
    /// <summary>
    /// 场景里的 MonoBehaviour。
    /// MonoBehaviour 不能用构造函数注入（Unity 负责 new），所以用 <c>[Inject]</c> 方法。
    /// </summary>
    public sealed class ExampleHud : MonoBehaviour
    {
        IScoreService m_ScoreService;
        string m_Message = "等待 VContainer 注入...";
        GUIStyle m_Style;

        /// <summary>
        /// <c>[Inject]</c> 方法在容器构建 / 对象被注入时调用，参数从容器解析。
        /// </summary>
        [Inject]
        public void Construct(IScoreService scoreService)
        {
            m_ScoreService = scoreService;
            m_ScoreService.ScoreChanged += OnScoreChanged;
            m_Message = "注入成功：IScoreService 已就绪";
            Debug.Log("[VContainer] ExampleHud 收到 [Inject] 注入");
        }

        void OnScoreChanged(int score)
        {
            m_Message = $"分数 +1 → {score}";
        }

        /// <summary>给 EntryPoint 调用的普通方法。</summary>
        public void ShowMessage(string message)
        {
            m_Message = message;
        }

        void OnDestroy()
        {
            if (m_ScoreService != null)
            {
                m_ScoreService.ScoreChanged -= OnScoreChanged;
            }
        }

        void OnGUI()
        {
            if (m_Style == null)
            {
                m_Style = new GUIStyle(GUI.skin.label)
                {
                    fontSize = 20,
                    fontStyle = FontStyle.Bold,
                    wordWrap = true,
                };
            }

            GUILayout.BeginArea(new Rect(24, 24, 620, 260), GUI.skin.box);
            GUILayout.Label("VContainer 示例", m_Style);
            GUILayout.Label($"分数：{(m_ScoreService == null ? "（未注入）" : m_ScoreService.Score.ToString())}", m_Style);
            GUILayout.Label($"状态：{m_Message}", m_Style);
            GUILayout.Label("提示：Bot 由 Func<Bot> 工厂创建，关闭 Play 时 Scope 会 Dispose。", m_Style);
            GUILayout.EndArea();
        }
    }
}
