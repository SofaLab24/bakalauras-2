using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

class DownscaleRenderPass : ScriptableRenderPass
{
    string profilerTag;

    Material materialToBlit;
    RenderTargetIdentifier cameraColorTargetIdent;

    int downsamples;
    float spread;
    int redColorCount;
    int greenColorCount;
    int blueColorCount;
    int bayerLevel;
    bool pointFilterDown;

    int width;
    int height;
    RenderTexture[] textures;
    RenderTexture currentSource;
    RenderTexture dither;


    public DownscaleRenderPass(string profilerTag,
      RenderPassEvent renderPassEvent, Material materialToBlit, int downsamples, float spread, int redColorCount, int greenColorCount, int blueColorCount, int bayerLevel, bool pointFilterDown)
    {
        this.profilerTag = profilerTag;
        this.renderPassEvent = renderPassEvent;
        this.materialToBlit = materialToBlit;

        this.downsamples = downsamples;
        this.spread = spread;
        this.redColorCount = redColorCount;
        this.greenColorCount = greenColorCount;
        this.blueColorCount = blueColorCount;
        this.bayerLevel = bayerLevel;
        this.pointFilterDown = pointFilterDown;
    }

    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        materialToBlit.SetFloat("_Spread", spread);
        materialToBlit.SetInt("_RedColorCount", redColorCount);
        materialToBlit.SetInt("_GreenColorCount", greenColorCount);
        materialToBlit.SetInt("_BlueColorCount", blueColorCount);
        materialToBlit.SetInt("_BayerLevel", bayerLevel);

        width = cameraTextureDescriptor.width;
        height = cameraTextureDescriptor.height;
        textures = new RenderTexture[downsamples];


        for (int i = 0; i < downsamples; ++i)
        {
            width /= 2;
            height /= 2;
            if (height < 2)
                break;
            textures[i] = RenderTexture.GetTemporary(width, height, 0);
        }
        dither = RenderTexture.GetTemporary(width, height, 0);

    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.Clear();

        for (int i = 0; i < textures.Length; ++i)
        {
            if (textures[i] == null) break;
            RenderTexture currentDestination = textures[i];
            if (i == 0)
            {
                if (pointFilterDown)
                    cmd.Blit(cameraColorTargetIdent, currentDestination, materialToBlit, 1);
                else
                    cmd.Blit(cameraColorTargetIdent, currentDestination);
            }
            else
            {
                if (pointFilterDown)
                    cmd.Blit(currentSource, currentDestination, materialToBlit, 1);
                else
                    cmd.Blit(currentSource, currentDestination);
            }

            currentSource = currentDestination;
        }

        cmd.Blit(currentSource, dither, materialToBlit, 0);
        cmd.Blit(dither, cameraColorTargetIdent, materialToBlit, 1);

        context.ExecuteCommandBuffer(cmd);

        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    public override void FrameCleanup(CommandBuffer cmd)
    {
        RenderTexture.ReleaseTemporary(dither);
        for (int i = 0; i < downsamples; ++i)
        {
            if (textures[i] != null)
            {
                RenderTexture.ReleaseTemporary(textures[i]);
            }
        }
    }
}