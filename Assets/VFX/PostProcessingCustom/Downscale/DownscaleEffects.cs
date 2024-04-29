using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DownscaleEffects : ScriptableRendererFeature
{
    [System.Serializable]
    public class DownscaleSettings
    {
        public bool IsEnabled = true;
        public RenderPassEvent WhenToInsert = RenderPassEvent.AfterRendering;
        public Material DownscaleMaterial;
        public int DownscaleAmount = 2;
        [Range(0f, 1f)]
        public float spread;
        public int redColorCount;
        public int greenColorCount;
        public int blueColorCount;
        public int bayerLevel;
        public bool pointFilterDown;

        public Material SharpnessMaterial;
        [Range(-10.0f, 10.0f)]
        public float sharpnessAmount = 2f;
    }

    public DownscaleSettings settings = new DownscaleSettings();

    SharpnessRenderPass sharpnessRenderPass;
    DownscaleRenderPass downscaleRenderPass;

    public override void Create()
    {
        sharpnessRenderPass = new SharpnessRenderPass(
            "Sharpness pass",
            settings.WhenToInsert,
            settings.SharpnessMaterial,
            settings.sharpnessAmount
        );

        downscaleRenderPass = new DownscaleRenderPass(
          "Downscaling pass",
          settings.WhenToInsert,
          settings.DownscaleMaterial,
          settings.DownscaleAmount,
          settings.spread,
          settings.redColorCount,
          settings.greenColorCount,
          settings.blueColorCount,
          settings.bayerLevel,
          settings.pointFilterDown
        );
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (!settings.IsEnabled)
        {
            return;
        }

        renderer.EnqueuePass(sharpnessRenderPass);
        renderer.EnqueuePass(downscaleRenderPass);
    }
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        var cameraColorTargetIdent = renderer.cameraColorTargetHandle;
        sharpnessRenderPass.Setup(cameraColorTargetIdent);
        downscaleRenderPass.Setup(cameraColorTargetIdent);
    }
}