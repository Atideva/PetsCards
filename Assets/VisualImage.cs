using __PUBLISH_v1.Scripts;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class VisualImage : MonoBehaviour
{
    [SerializeField] VisualType type;
    [SerializeField] GameVisual visualConfig;
    [SerializeField] Image image;
    [SerializeField] SpriteRenderer sprite;


    void Start()
    {
        if (!image) image = GetComponent<Image>();
        if (!sprite) sprite = GetComponent<SpriteRenderer>();
        if (!Application.isPlaying) return;
        if (image) image.sprite = GameManager.Instance.Config.Visual.GetSprite(type);
        if (sprite) sprite.sprite = GameManager.Instance.Config.Visual.GetSprite(type);
    }

#if UNITY_EDITOR
    void Update()
    {
        if (Application.isPlaying) return;
        if (visualConfig)
        {
            if (image || sprite)
            {
                if (image) image.sprite = visualConfig.GetSprite(type);
                if (sprite) sprite.sprite = visualConfig.GetSprite(type);
            }
            else
                Debug.Log("Image has not been set", gameObject);
        }
        else
            Debug.Log("Visual config is not set for image", gameObject);
    }
#endif
}