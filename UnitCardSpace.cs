using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xcom_tactics
{
    internal class UnitCardSpace : CardSpace
    {
        public delegate void UnitPlaced(Unit subject_, Unit object_);

        public UnitPlaced? OnUnitPlaced { get; set; }

        Dictionary<Unit, Card> m_unitCards = [];

        Dictionary<Unit, CardSlot> m_unitSlots = [];

        Dictionary<Unit, CardSlot> m_interactionSlots = [];

        public UnitCardSpace()
        {
            OnCardPlaced = onCardPlaced;
        }

        public void SetUnits(List<Unit> enemyUnits_, List<Unit> ourUnits_)
        {
            int horizontalInterval = (Width - enemyUnits_.Count * Card.g_cardSize.Width) / (enemyUnits_.Count + 1);
            int verticalInterval = (Height / 2 - Card.g_cardSize.Height * 2) / 3;
            var availableSlots = new List<CardSlot>();
            var x = horizontalInterval + Card.g_cardSize.Width / 2;
            var y = verticalInterval + Card.g_cardSize.Height / 2;
            for (var i = 0; i < enemyUnits_.Count; ++i)
            {
                var slot1 = new CardSlot { Location = new Point(x, y) };
                AddSlot(slot1);
                var card = new UnitCard(enemyUnits_[i]);
                m_unitCards[enemyUnits_[i]] = card;
                AddCard(card, slot1);
                m_unitSlots[enemyUnits_[i]] = slot1;

                var slot2 = new CardSlot { Location = new Point(x, y + Card.g_cardSize.Height + verticalInterval) };
                AddSlot(slot2);
                //AddInteractionSlot(card, slot2);
                m_interactionSlots[enemyUnits_[i]] = slot2;
                availableSlots.Add(slot2);

                x += horizontalInterval;
            }

            horizontalInterval = (Width - ourUnits_.Count * Card.g_cardSize.Width) / (ourUnits_.Count + 1);
            x = horizontalInterval + Card.g_cardSize.Width / 2;
            y = Height / 2 + verticalInterval + Card.g_cardSize.Height / 2;
            for (var i = 0; i < ourUnits_.Count; ++i)
            {
                var slot1 = new CardSlot { Location = new Point(x, y) };
                AddSlot(slot1);
                var card = new UnitCard(ourUnits_[i]);
                m_unitCards[ourUnits_[i]] = card;
                AddCard(card, slot1);
                m_unitSlots[ourUnits_[i]] = slot1;

                var slot2 = new CardSlot { Location = new Point(x, y + Card.g_cardSize.Height + verticalInterval) };
                AddSlot(slot2);
                //AddInteractionSlot(card, slot2);
                m_interactionSlots[ourUnits_[i]] = slot2;
                AddAvailableSlots(card, availableSlots);

                x += horizontalInterval;
            }
        }

        public void RemoveUnit(Unit unit_)
        {
            if (!m_unitCards.ContainsKey(unit_))
                return;
            var card = m_unitCards[unit_];
            RemoveCard(card);
            RemoveSlot(m_unitSlots[unit_]);
            RemoveSlot(m_interactionSlots[unit_]);
        }

        void onCardPlaced(Card card_, CardSlot slot_)
        {
            var subjectUnitCard = card_ as UnitCard;
            if (subjectUnitCard == null)
                return;
            if (!m_interactionSlots.ContainsValue(slot_))
                return;
            var objectUnitAndSlot = m_interactionSlots.First(s => s.Value == slot_);
            var objectUnit = m_interactionSlots.First(s => s.Value == slot_).Key;
            if (objectUnit == null)
                return;
            OnUnitPlaced(subjectUnitCard.Unit, objectUnit);
        }
    }
}
