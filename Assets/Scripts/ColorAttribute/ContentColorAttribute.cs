 
using UnityEngine;

public class ContentColorAttribute : PropertyAttribute
{
    public float r;
    public float g;
    public float b;
    public float a;
    public ContentColorAttribute()
    {
        r = g = b = a = 1f;
    }
    public ContentColorAttribute(float aR, float aG, float aB, float aA)
    {
        r = aR; 
        g = aG;
        b = aB;
        a = aA;
    }
    public Color color { get { return new Color(r, g, b, a); } }
}
