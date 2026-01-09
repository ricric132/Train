using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
[RequireComponent(typeof(Camera))]
public class CustomRect : MonoBehaviour
{
    public float rectX; //all as fraction of screen
    public float rectY;
    public float rectWidth;
    public float rectHeight;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //public static void CopyTexture(Rendering.GraphicsTexture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Rendering.GraphicsTexture dst, int dstElement, int dstMip, int dstX, int dstY);
        Graphics.CopyTexture(source, 
            0, 
            0, 
            (int)(rectX * source.width), 
            (int)(rectY * source.height), 
            (int)(rectWidth * source.width), 
            (int)(rectHeight * source.height),
            destination,
            0,
            0,
            (int)(rectX * destination.width),
            (int)(rectY * destination.height)
        );

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
