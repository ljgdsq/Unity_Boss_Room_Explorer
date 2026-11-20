using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.BossRoom.Examples.VContainerExample.EditorTools
{
    /// <summary>
    /// 一键创建 VContainer 示例场景：相机 + 灯光 + Scope（自动挂 HUD）。
    /// 菜单：Tools/Boss Room/VContainer 示例/创建示例场景
    /// </summary>
    public static class VContainerExampleSceneBuilder
    {
        const string k_ScenePath = "Assets/Scenes/VContainerExample.unity";

        [MenuItem("Tools/Boss Room/VContainer 示例/创建示例场景")]
        public static void CreateScene()
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                return;
            }

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateCamera();
            CreateLight();

            var scopeObject = new GameObject("VContainerExample");
            var hud = scopeObject.AddComponent<ExampleHud>();
            var scope = scopeObject.AddComponent<ExampleSceneScope>();

            // 把 HUD 序列化引用写进 Scope，省去手动拖拽
            var serializedScope = new SerializedObject(scope);
            var hudProperty = serializedScope.FindProperty("m_Hud");
            if (hudProperty != null)
            {
                hudProperty.objectReferenceValue = hud;
                serializedScope.ApplyModifiedPropertiesWithoutUndo();
            }

            EditorSceneManager.SaveScene(scene, k_ScenePath);
            AddSceneToBuildSettings(k_ScenePath);
            AssetDatabase.Refresh();

            Selection.activeGameObject = scopeObject;
            Debug.Log($"[VContainer] 示例场景已创建：{k_ScenePath}，按 Play 运行。");
        }

        static void CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 7f, -9f);
            cameraObject.transform.rotation = Quaternion.Euler(35f, 0f, 0f);

            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.09f, 0.10f, 0.14f);
            cameraObject.AddComponent<AudioListener>();
        }

        static void CreateLight()
        {
            var lightObject = new GameObject("Directional Light");
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            light.color = new Color(1f, 0.96f, 0.9f);
        }

        // 把示例场景加入 Build Settings，方便之后从打包版直接验证
        static void AddSceneToBuildSettings(string scenePath)
        {
            var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            foreach (var existing in scenes)
            {
                if (existing.path == scenePath)
                {
                    return;
                }
            }

            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
}
