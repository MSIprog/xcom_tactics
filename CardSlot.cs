using System.Drawing.Drawing2D;

namespace xcom_tactics
{
    public class CardSlot
    {
        public Point Location { get; set; }

        public Size Size { get; set; }

        public Rectangle Bounds
        {
            get
            {
                var result = new Rectangle(Point.Empty, Size);
                Utils.SetCenter(ref result, Location);
                return result;
            }
        }

        public CardSlot(int width_ = 1)
        {
            Size = new Size(width_ * Card.g_cardSize.Width, Card.g_cardSize.Height);
        }

        public void Draw(Graphics graphics_)
        {
            var rect = new Rectangle(Point.Empty, Size);
            Utils.SetCenter(ref rect, Location);
            graphics_.FillRectangle(new HatchBrush(HatchStyle.DiagonalCross, Color.Gray, Color.Transparent), rect);
            graphics_.DrawRectangle(new Pen(Color.Black), rect);
        }
    }
}
