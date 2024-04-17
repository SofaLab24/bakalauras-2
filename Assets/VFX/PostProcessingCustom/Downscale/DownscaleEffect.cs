using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DownscaleEffect : ScriptableRendererFeature
{
    [System.Serializable]
    public class DownscaleSettings
    {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material MaterialToBlit;
        public int DownscaleAmount = 2;
    }

    // MUST be named "settings" (lowercase) to be shown in the Render Features inspector
    public DownscaleSettings settings = new DownscaleSettings();

    RenderTargetHandle renderTextureHandle;
    DownscaleRenderPass downscaleRenderPass;

    public override void Create()
    {
        downscaleRenderPass = new DownscaleRenderPass(
          "Downscaling pass",
          settings.WhenToInsert,
          settings.MaterialToBlit,
          settings.DownscaleAmount
        );
    }

    // called every frame once per camera
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!settings.IsEnabled)
        {
            // we can do nothing this frame if we want
            return;
        }

        // Ask the renderer to add our pass.
        // Could queue up multiple passes and/or pick passes to use
        renderer.EnqueuePass(downscaleRenderPass);
    }
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        // Gather up and pass any extra information our pass will need.
        // In this case we're getting the camera's color buffer target
        var cameraColorTargetIdent = renderer.cameraColorTarget;
        downscaleRenderPass.Setup(cameraColorTargetIdent);
    }
}