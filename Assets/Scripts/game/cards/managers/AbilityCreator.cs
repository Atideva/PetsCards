using System.Collections.Generic;
using game.cards.data;
using Misc;
using UnityEngine;

namespace game.cards.managers
{
    public class AbilityCreator : MonoBehaviour
    {
        [SerializeField] private Color goodAbilityColor;
        [SerializeField] private Color evilAbilityColor;


        const string CONTAINER_NAME = "Card abilites";
        Color GetColor(AbilityType type) => type == AbilityType.Evil ? evilAbilityColor : goodAbilityColor;
        Dictionary<AbilityConfig, AbilityActivator> _abilities = new();
        List<CardData> _outlines = new();
        Transform _container;

        void Awake()
        {
            _container = new GameObject(CONTAINER_NAME).transform;
        }



        public void CreateAbility(CardData card)
        {
            if (card.Abilities.Count == 0) return;


            foreach (var ability in card.Abilities)
            {
                if (!_abilities.ContainsKey(ability.config))
                {
                    var container = CreateContainer(card.name);
                    CreateOutline(card, container);
                    CreateActivator(ability, container)
                        .With(activator => _abilities.Add(ability.config, activator))
                        .With(activator => activator.AddTrigger(card));
     
                }
                else
                {
                    _abilities[ability.config].AddTrigger(card);
                }
            }

            _container.name = CONTAINER_NAME + " (" + _abilities.Count + ")";
        }

        private void CreateOutline(CardData card, Transform container)
        {
            if (_outlines.Contains(card)) return;
            _outlines.Add(card);
            container.gameObject.AddComponent<AbilityCardOutline>()
                .With(g => g.Init(card, GetColor(card.Abilities[0].config.Type)));
        }


        Transform CreateContainer(string newName)
            => new GameObject(newName).transform
                .With(t => t.SetParent(_container));

        AbilityActivator CreateActivator(AbilityData data, Transform container)
            => Instantiate(data.config.Prefab, container)
                .With(p => p.gameObject.name = data.config.name)
                .With(p => p.Init(data.config, data.trigger));
    }
}