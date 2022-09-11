using UnityEngine;

namespace game.cards
{
    public class FakeShadow : MonoBehaviour
    {
        [SerializeField] SpriteRenderer shadow;
        Transform _parent;
        Vector3 _offset;

        void Start()
        {
            _offset = transform.localPosition;
            _parent = transform.parent;
        }

        public void Enable()
        {
            Invoke(nameof(UnParent), 0.5f);
            // use card placed on table event please, instead of invoke
            Debug.LogWarning("Please check it!");
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
        bool _unParent;

        void UnParent()
        {
            _unParent = true;
            transform.parent = null;
        }

        void FixedUpdate()
        {
            if (!_unParent) return;

            transform.rotation = _parent.rotation;
            //transform.position = parent.position + offset;
            if (!_parent.gameObject.activeInHierarchy) gameObject.SetActive(false);
        }
    }
}