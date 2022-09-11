using UnityEngine;

namespace app.settings
{
    [ExecuteInEditMode]
    public class EditorCanvasGizmos : MonoBehaviour
    {
#if UNITY_EDITOR
        [Header("Color")] public Color clr;
        [SerializeField] Canvas canvasParent;
        [Header("Inspector")] public bool autoOffset;
        public int xOffset;
        public int yOffset;
        [Header("Resolution info:")] public float width;
        public float height;
        RectTransform _rect;
        float _x, _y;
        void Start() => _rect = GetComponent<RectTransform>();


        void Update()
        {
            if (Application.isPlaying)
            {
                return;
            }
        
            width = _rect.rect.width* canvasParent.transform.localScale.x;
            height = _rect.rect.height* canvasParent.transform.localScale.y;


            if (autoOffset)
            {
                var xNew = xOffset * width;
                var yNew = yOffset * height;
                var movePos = new Vector3(xNew, yNew, 0);

                if (_rect.localPosition != movePos)
                {
                    _rect.localPosition = movePos;
                }
            }

            var rect = (RectTransform) transform;
            _x = transform.position.x;
            _y = transform.position.y;
            DrawQuadrant();
        }

        void DrawQuadrant()
        {
            float w = width / 2;
            float h = height / 2;

            Vector2 a = new Vector2(_x - w, _y - h);
            Vector2 b = new Vector2(_x - w, _y + h);
            Vector2 c = new Vector2(_x + w, _y + h);
            Vector2 d = new Vector2(_x + w, _y - h);

            Debug.DrawLine(a, b, clr);
            Debug.DrawLine(b, c, clr);
            Debug.DrawLine(c, d, clr);
            Debug.DrawLine(d, a, clr);
        }
#endif
    }
}