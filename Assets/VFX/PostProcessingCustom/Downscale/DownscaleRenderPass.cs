using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

class DownscaleRenderPass : ScriptableRenderPass
{
    // used to label this pass in Unity's Frame Debug utility
    string profilerTag;

    Material materialToBlit;
    RenderTargetIdentifier cameraColorTargetIdent;
    RenderTargetHandle tempTexture;
    int downsamples;

    int width;
    int height;
    RenderTexture[] textures;
    RenderTexture currentSource;


    public DownscaleRenderPass(string profilerTag,
      RenderPassEvent renderPassEvent, Material materialToBlit, int downsamples)
    {
        this.profilerTag = profilerTag;
        this.renderPassEvent = renderPassEvent;
        this.materialToBlit = materialToBlit;
        this.downsamples = downsamples;
    }

    // This isn't part of the ScriptableRenderPass class and is our own addition.
    // For this custom pass we need the camera's color target, so that gets passed in.
    public void Setup(RenderTargetIdentifier cameraColorTargetIdent)
    {
        this.cameraColorTargetIdent = cameraColorTargetIdent;
    }

    // called each frame before Execute, use it to set up things the pass will need
    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        // create a temporary render texture that matches the camera
        // cmd.GetTemporaryRT(tempTexture.id, cameraTextureDescriptor);

        width = cameraTextureDescriptor.width;
        height = cameraTextureDescriptor.height;
        textures = new RenderTexture[downsamples];


        for (int i = 0; i < downsamples; ++i)
        {
            width /= 2;
            height /= 2;
            if (height < 2)
                break;

            RenderTexture currentDestination = textures[i] = RenderTexture.GetTemporary(width, height, 0);
            currentSource = currentDestination;
        }
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);
        cmd.Clear();
        
        for (int i = 0; i < textures.Length; ++i)
        {
            cmd.Blit(cameraColorTargetIdent, textures[i], materialToBlit);
        }
        cmd.Blit(currentSource, cameraColorTargetIdent, materialToBlit);

        context.ExecuteCommandBuffer(cmd);

        // tidy up after ourselves
        cmd.Clear();
        CommandBufferPool.Release(cmd);
    }

    // called after Execute, use it to clean up anything allocated in Configure
    public override void FrameCleanup(CommandBuffer cmd)
    {
        for (int i = 0; i < downsamples; ++i)
        {
            RenderTexture.ReleaseTemporary(textures[i]);
        }
        cmd.ReleaseTemporaryRT(tempTexture.id);
    }
}