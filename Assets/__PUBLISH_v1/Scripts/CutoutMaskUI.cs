using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUI : Image
{
    static readonly int StencilComp = Shader.PropertyToID("_StencilComp");

    public override Material materialForRendering
    {
        get
        {
            var rendering = new Material(base.materialForRendering);
            rendering.SetInt(StencilComp, (int) CompareFunction.NotEqual);
            return rendering;
        }
    }
}