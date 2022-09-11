using System.Collections.Generic;
using Cinemachine;
using game.cards;
using game.cards.data;
using game.managers;
using UnityEngine;

namespace systems.camera
{
    public class CameraTargetGroup : MonoBehaviour
    {
        public CinemachineTargetGroup group;
        public float clearGroupAfterSec;

        [Header("Card size")]
        public float cardXsize;
        public float cardYsize;
        public float xOffset;
        public float yOffset;

        [Header("Setup")]
        public bool useCornerObjects;
        public Transform topLeft;
        public Transform topRight;
        public Transform bottomLeft;
        public Transform bottomRight;

        public float leftX;
        public float rightX;
        public float topY;
        public float bottomY;

        List<Transform> _remove;

        void Start()
        {
            Events.Instance.OnRoundWin += SessionWin;

            if (useCornerObjects)
            {
                Events.Instance.OnCardPositionChange += CardPosition;
            }
            else
            {
                Events.Instance.OnPairCreate += PairCreate;
            }
        }

        void CardPosition(Card card, Vector3 pos)
        {
            CheckCorners(pos);
            SetCornerPositons();
        }

        void SessionWin(int pairs)
        {
            if (useCornerObjects)
            {
                leftX = rightX = topY = bottomY = 0;
            }
            else
            {
                if (clearGroupAfterSec != 0)
                {
                    _remove = new List<Transform>();
                    foreach (var item in group.m_Targets)
                    {
                        _remove.Add(item.target);
                    }

                    Invoke(nameof(ClearDelayed), clearGroupAfterSec);
                }
                else
                {
                    Clear();
                }
            }
        }

        void PairCreate(Card card1, Card card2, CardData cardType, DeckData deck)
        {
            group.AddMember(card1.transform, 1, 0);
            group.AddMember(card2.transform, 1, 0);
        }

        void CheckCorners(Vector2 position)
        {
            var xMin = position.x - cardXsize;
            var xMax = position.x + cardXsize;
            var yMin = position.y - cardYsize;
            var yMax = position.y + cardYsize;
            CheckCorners_X(xMin, xMax);
            CheckCorners_Y(yMin, yMax);
        }

        void CheckCorners_X(float xMin, float xMax)
        {
            if (xMin < leftX) leftX = xMin - xOffset;
            if (xMax > rightX) rightX = xMax + xOffset;
        }

        void CheckCorners_Y(float yMin, float yMax)
        {
            if (yMin < bottomY) bottomY = yMin - yOffset;
            if (yMax > topY) topY = yMax + yOffset;
        }

        void SetCornerPositons()
        {
            topLeft.position = new Vector2(leftX, topY);
            topRight.position = new Vector2(rightX, topY);
            bottomLeft.position = new Vector2(leftX, bottomY);
            bottomRight.position = new Vector2(rightX, bottomY);
        }

        void Clear()
        {
            foreach (var item in group.m_Targets)
            {
                group.RemoveMember(item.target);
            }
        }

        void ClearDelayed()
        {
            foreach (var item in _remove)
            {
                group.RemoveMember(item);
            }
        }
    }
}