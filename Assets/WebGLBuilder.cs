#if UNITY_EDITOR

using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[UsedImplicitly]
public static class WebGLBuilder
{
    public static void Build()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "Build", BuildTarget.WebGL, BuildOptions.None);
    }
}

#endif