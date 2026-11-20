using System;
using UnityEngine;

namespace Unity.BossRoom.Examples.VContainerExample
{
    /// <summary>
    /// 由工厂创建的对象：数量不固定、也不需要全局唯一，
    /// 所以不注册进容器，而是注册一个 <c>Func&lt;Bot&gt;</c> 工厂。
    /// </summary>
    public sealed class Bot : IDisposable
    {
        readonly float m_SpinSpeed;

        public Transform Transform { get; }

        public Bot(Transform transform, float spinSpeed)
        {
            Transform = transform;
            m_SpinSpeed = spinSpeed;
        }

        public void Tick(float deltaTime)
        {
            if (Transform != null)
            {
                Transform.Rotate(Vector3.up, m_SpinSpeed * deltaTime, Space.World);
            }
        }

        public void Dispose()
        {
            if (Transform != null)
            {
                UnityEngine.Object.Destroy(Transform.gameObject);
            }
        }
    }
}
