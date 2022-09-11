using System.Collections.Generic;
using UnityEngine;

namespace game.cards.managers
{
    public class StatusManager : MonoBehaviour
    {
        #region Singleton
        //-------------------------------------------------------------
        public static StatusManager Instance;
        void Awake()
        {
            if (Instance == null) Instance = this;
            else
            {
                Debug.LogError("Did you open it in prefab mode, or there's really 2 instanses of Singleton here?", gameObject);
                //gameObject.SetActive(false);
            }
        }
        //-------------------------------------------------------------
        #endregion

        Dictionary<GameObject, Status> _cardStatus = new Dictionary<GameObject, Status>();

        void Start()
        {
            //EventManager.Instance.OnSession_Pair_PairFinded += PairOpened;
        }

        //  void PairOpened(GameObject card1, GameObject card2)
        //{
        //    throw new NotImplementedException();
        //}

        void SetCardStatus(GameObject card, Status status)
        {
            if (_cardStatus.ContainsKey(card))
                _cardStatus[card] = status;
            else
                _cardStatus.Add(card, status);
        }
        public Status GetCardStatus(GameObject card)
        {
            if (!_cardStatus.ContainsKey(card))
            {
                _cardStatus.Add(card, Status.Close);
                Debug.LogError("Requested card isnt in status dictionary!");
            }
            return _cardStatus[card];
        }


        public void SetStatus_Open(GameObject card) => SetCardStatus(card, Status.Open);
        public void SetStatus_Close(GameObject card) => SetCardStatus(card, Status.Close);


    }
}