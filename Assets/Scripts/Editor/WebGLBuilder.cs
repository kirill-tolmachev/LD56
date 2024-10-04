#if UNITY_EDITOR

using JetBrains.Annotations;
using UnityEditor;
// ReSharper disable CheckNamespace

[UsedImplicitly]
public static class WebGLBuilder
{
    [UsedImplicitly]
    public static void Build()
    {
        string[] scenes = { "Assets/Scenes/SampleScene.unity" };
        BuildPipeline.BuildPlayer(scenes, "/Users/mbp/Projects/LD56-Meta/webgl_build", BuildTarget.WebGL, BuildOptions.None);
    }
}

#endif