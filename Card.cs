namespace xcom_tactics
{
    internal class StatusLabel
    {
        required public string Text { get; set; }

        public Color ForegroundColor { get; set; }

        public Color BackgroudColor { get; set; }
    }

    internal partial class Card
    {
        public static Size g_cardSize = new Size(72, 120); // 90, 150

        public static int g_cardRoundSize = 10;

        private List<StatusLabel> statusLabels = [];
        public List<StatusLabel> StatusLabels
        {
            get => statusLabels;
            set => statusLabels = value;
        }

        // центр карты
        public Point Location { get; set; }

        //public Point AnimationLocation { get; set; }

        public bool Selected { get; set; }

        public Rectangle Bounds
        {
            get
            {
                var result = new Rectangle(Point.Empty, g_cardSize);
                Utils.SetCenter(ref result, Location);
                return result;
            }
        }

        static Card()
        {
        }

        public virtual void Draw(Graphics graphics_)
        {
            var location = Location;
            /*if (animationFactor_ == 1)
            {
                AnimationLocation = Location;
                AnimationVisible = Visible;
            }
            else
            {
                if (Location == AnimationLocation)
                    animationFactor_ = 1;
                else
                    location = Point.Ceiling(new PointF((Location.X - AnimationLocation.X) * animationFactor_ + AnimationLocation.X, (Location.Y - AnimationLocation.Y) * animationFactor_ + AnimationLocation.Y));
            }*/

            // frame
            var frameRect = new Rectangle(Point.Empty, g_cardSize);
            Utils.SetCenter(ref frameRect, location);
            graphics_.DrawRectangle(new Pen(Color.LightBlue), frameRect);

            // status string
            /*frameRect = new Rectangle(infoRect.Left - 5, infoRect.Top - 25, infoRect.Width + 10, 20);
            foreach (var status in StatusLabels)
            {
                var statusBrush = new SolidBrush(status.BackgroudColor);
                graphics_.FillRectangle(statusBrush, frameRect);
                statusBrush = new SolidBrush(status.ForegroundColor);
                FontFamily fontFamily;
                try
                {
                    fontFamily = new FontFamily("Arial Narrow");
                }
                catch (ArgumentException)
                {
                    fontFamily = FontFamily.GenericSansSerif;
                }
                var font = new Font(fontFamily, 8);
                var format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                graphics_.DrawString(status.Text, font, statusBrush, frameRect, format);
                frameRect.Y -= 25;
            }*/
        }
    }
}
