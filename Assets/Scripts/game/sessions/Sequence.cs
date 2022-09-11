using System.Collections;
using System.Collections.Generic;
using System.Linq;
using __PUBLISH_v1.Scripts;
using game.managers;
using game.sessions.type;
using Misc;
using UnityEngine;

namespace game.sessions
{
    [ExecuteInEditMode]
    public class Sequence : MonoBehaviour
    {
        #region Singleton

        //-------------------------------------------------------------
        public static Sequence Instance;

        void Awake()
        {
            if (Instance == null) Instance = this;
            else gameObject.SetActive(false);
        }

        //-------------------------------------------------------------

        #endregion

        [Header("DEBUG")]
        [SerializeField] List<Transform> sessions = new();
        List<ISession> _iSessions = new();
        int _current;


        public IReadOnlyList<FindPairSession> GetPairSessions()
        {
            List<FindPairSession> newList = new();
            foreach (var iSession in _iSessions)
            {
                if (iSession is FindPairSession p) newList.Add(p);
            }

            return newList;
        }

        public int TotalSessions => _iSessions.Count(item => item is not HitpointMode && item is not TimerMode);

        void Start()
        {
            if (!Application.isPlaying) return;

            Events.Instance.OnSessionComplete += SessionComplete;

            var delay = GameManager.Instance.Config.Settings.FirstSessionDelay;
            Invoke(nameof(CheckLevelMode), delay < 0.2f ? 0.2f : delay);
            RefreshList();
            SendRequests();
            Invoke(nameof(StartSequence), GameManager.Instance.Config.Settings.FirstSessionDelay);
        }

        void CheckLevelMode()
        {
            switch (GameManager.Instance.CurrentLevel.Mode)
            {
                case LevelMode.Timer:
                    Events.Instance.EnableTimer();
                    break;
                case LevelMode.Lives:
                    Events.Instance.HitpointsEnable();
                    break;
            }
        }

        public bool waitSlider;
        public bool sliderDone;

        void SessionComplete() 
            => StartCoroutine(StartNextSequence());

        IEnumerator StartNextSequence()
        {
            if (waitSlider) yield return new WaitUntil(() => sliderDone);
            sliderDone = false;
            _current++;
            if (_current >= _iSessions.Count) yield break;
            _iSessions[_current].StartSession();
        }

 

        void StartSequence()
        {
            _current = 0;
            _iSessions[_current].StartSession();
        }


        void RefreshList()
        {
            Clear();
            FindSessionsInChild();
            AddSessionsFromConfig();
        }

        void Clear()
        {
            sessions = new List<Transform>();
            _iSessions = new List<ISession>();
        }

        void FindSessionsInChild()
        {
            var child = GetComponentsInChildren<Transform>();
            foreach (var item in child)
            {
                var iSession = item.GetComponent<ISession>();
                if (iSession == null) continue;

                if (!sessions.Contains(item)) sessions.Add(item);
                if (!_iSessions.Contains(iSession)) _iSessions.Add(iSession);
            }
        }

        void AddSessionsFromConfig()
        {
            var i = 0;
            foreach (var deck in GameManager.Instance.CurrentLevel.Decks)
            {
                i++;
                new GameObject()
                    .With(g => g.name = "FindPair " + i)
                    .With(g => g.transform.SetParent(transform))
                    .AddComponent<FindPairSession>()
                    .With(session => sessions.Add(session.transform))
                    .With(session => _iSessions.Add(session))
                    .With(session => session
                        .AddSessionDeck(new SessionDeck()
                            .With(s => s.deck = deck)
                            .With(s => s.pairs = deck.Cards.Count)));
            }
        }

        void SendRequests()
        {
            foreach (var item in _iSessions) item.Request();
        }
    }
}