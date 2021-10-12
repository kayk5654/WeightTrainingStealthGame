using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderDepthFeature : ScriptableRendererFeature
{
    class RenderDepthPass : ScriptableRenderPass
    {
        // material for post effect
        public Material _material;
        
        /// <summary>
        /// actual rendering logic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="renderingData"></param>
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (!_material) { return; }

            // get camera to render
            Camera camera = renderingData.cameraData.camera;
            
            // get command buffer to calculate redner result
            CommandBuffer cmd = CommandBufferPool.Get("RenderDepth");

            // add a rendering command to blit a render texture
            // apply the material for the camera
            cmd.Blit(Texture2D.whiteTexture, camera.activeTexture, _material);

            // execute command buffer declared above
            context.ExecuteCommandBuffer(cmd);

            context.Submit();
        }
    }

    RenderDepthPass _renderDepthPass;

    // material for post effect
    public Material _material;

    /// <summary>
    /// create an instance of render pass, set references on it
    /// </summary>
    public override void Create()
    {
        _renderDepthPass = new RenderDepthPass();

        // load material for post effect, then assign in for the render pass
        _renderDepthPass._material = _material;


        // Configures where the render pass should be injected.
        _renderDepthPass.renderPassEvent = RenderPassEvent.AfterRendering+2;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_renderDepthPass);
    }
}


