using UnityEngine;
using UnityEditor;
 
[CustomPropertyDrawer(typeof(ContentColorAttribute))]
public class ContentColorDecorator : DecoratorDrawer
{
    ContentColorAttribute attr { get { return ((ContentColorAttribute)attribute); } }
    public override float GetHeight() { return 0; }
 
    public override void OnGUI(Rect position)
    {
        // GUI.backgroundColor = attr.color;
        GUI.contentColor = attr.color;
    }
}
