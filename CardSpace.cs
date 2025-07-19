using System.Drawing.Drawing2D;

namespace xcom_tactics
{
    internal partial class CardSpace : UserControl
    {
        public delegate void CardPlaced(Card card_, CardSlot slot_);

        /*static Pen m_blackPen = new Pen(Color.Black, 1);
        static Brush m_textBrush = new SolidBrush(Color.Black);*/

        List<Card> m_cards = new List<Card>();

        List<CardSlot> m_slots = new List<CardSlot>();

        // слоты, в которых находятся карты
        Dictionary<Card, CardSlot> m_cardsInSlots = [];

        // куда можно положить карту
        Dictionary<Card, List<CardSlot>> m_availableSlots = [];

        // интерактивный слот рядом картой (в который кладется другая карта)
        // todo: del
        //Dictionary<Card, CardSlot> m_interactionSlots = [];

        // text at the top
        //public List<string>? TextLines { private get; set; }

        Card? m_draggingCard;

        Point m_draggingCardOffset;

        public CardPlaced? OnCardPlaced { get; set; }

        public CardSpace()
        {
            InitializeComponent();
        }

        public void AddSlot(CardSlot slot_)
        {
            m_slots.Add(slot_);
        }

        public void AddCard(Card card_, CardSlot slot_)
        {
            m_cards.Add(card_);
            card_.Location = slot_.Location;
            m_cardsInSlots[card_] = slot_;
        }

        // todo: del
        /*public void AddInteractionSlot(Card card_, CardSlot slot_)
        {
            m_interactionSlots[card_] = slot_;
        }*/

        public void AddAvailableSlots(Card card_, List<CardSlot> slots_)
        {
            m_availableSlots[card_] = slots_;
        }

        public CardSlot? GetSlot(Card card_)
        {
            if (!m_cardsInSlots.ContainsKey(card_))
                return null;
            return m_cardsInSlots[card_];
        }

        /*public Card? GetInteractionCard(CardSlot slot_)
        {
            if (!m_interactionSlots.ContainsValue(slot_))
                return null;
            return m_interactionSlots.First(s => s.Value == slot_).Key;
        }

        public CardSlot? GetInteractionSlot(Card card_)
        {
            if (!m_interactionSlots.ContainsKey(card_))
                return null;
            return m_interactionSlots[card_];
        }*/

        public void RemoveCard(Card card_)
        {
            m_cards.Remove(card_);
            m_cardsInSlots.Remove(card_);
        }

        public void RemoveSlot(CardSlot slot_)
        {
            m_slots.Remove(slot_);
            if (m_cardsInSlots.ContainsValue(slot_))
                throw new Exception("Not empty slot");
        }

        private void resized(object sender, EventArgs e)
        {
        }

        private void painted(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.FillRectangle(new SolidBrush(Color.LightBlue), 0, 0, Width, Height);
            e.Graphics.DrawLine(new Pen(Color.Gray), 0, Height / 2, Width, Height / 2);

            foreach (var slot in m_slots)
                slot.Draw(e.Graphics);

            foreach (var card in m_cards)
                card.Draw(e.Graphics);

            /*if (TextLines != null)
            {
                var font = new Font(FontFamily.GenericSansSerif, 14);
                var textColor = new SolidBrush(Color.DarkRed);
                var labels = new List<string>();
                var values = new List<string>();
                TextLines.ForEach(l =>
                {
                    var separatorIndex = l.IndexOf(": ");
                    if (separatorIndex == -1)
                    {
                        labels.Add(string.Empty);
                        values.Add(l);
                    }
                    else
                    {
                        labels.Add(l.Substring(0, separatorIndex + 2));
                        values.Add(l.Substring(separatorIndex + 2));
                    }
                });
                var margin = 30;
                e.Graphics.DrawString(string.Join(Environment.NewLine, labels), font, textColor,
                    new Rectangle(0, margin, Width / 2, Height / 2 - margin * 2),
                    new StringFormat { Alignment = StringAlignment.Far, LineAlignment = StringAlignment.Near });
                e.Graphics.DrawString(string.Join(Environment.NewLine, values), font, textColor,
                    new Rectangle(Width / 2, margin, Width / 2, Height / 2 - margin * 2),
                    new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near });
            }*/
        }

        private void mousePressed(object sender, MouseEventArgs e)
        {
            m_draggingCard = m_cards.Find(c => c.Bounds.Contains(e.Location));
            if (m_draggingCard == null)
                return;
            if (!m_availableSlots.ContainsKey(m_draggingCard))
            {
                m_draggingCard = null;
                return;
            }
            m_draggingCardOffset = e.Location - new Size(m_draggingCard.Location);
        }

        private void mouseMoved(object sender, MouseEventArgs e)
        {
            if (m_draggingCard == null)
                return;
            m_draggingCard.Location = e.Location - new Size(m_draggingCardOffset);
            Invalidate();
        }

        private void mouseReleased(object sender, MouseEventArgs e)
        {
            if (m_draggingCard == null)
                return;
            if (!m_availableSlots.ContainsKey(m_draggingCard))
            {
                m_draggingCard.Location = m_cardsInSlots[m_draggingCard].Location;
                m_draggingCard = null;
                return;
            }
            var slots = m_availableSlots[m_draggingCard];
            var slot = slots.Find(s => s.Bounds.Contains(m_draggingCard.Location));
            if (slot == null)
                m_draggingCard.Location = m_cardsInSlots[m_draggingCard].Location;
            else
            {
                m_draggingCard.Location = slot.Location;
                m_cardsInSlots[m_draggingCard] = slot;
                if (OnCardPlaced != null && slot != null)
                    OnCardPlaced(m_draggingCard, slot);
            }

            m_draggingCard = null;
            Invalidate();
        }

        /*private void animationTimer_Tick(object sender, EventArgs e)
        {
            var timeNow = DateTime.Now;
            if ((timeNow - m_animationStartTime).TotalMilliseconds > g_animationTime)
            {
                animationTimer.Stop();
                m_animationFactor = 1;
                m_cards.ToList().ForEach(cards => cards.ForEach(c => c.AnimationLocation = c.Location));
            }
            else
                m_animationFactor = (float)(timeNow - m_animationStartTime).TotalMilliseconds / g_animationTime;
            Invalidate();
        }*/
    }
}
