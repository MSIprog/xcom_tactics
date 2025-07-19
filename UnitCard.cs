using System.Resources;

namespace xcom_tactics
{
    internal class UnitCard : Card
    {
        static Dictionary<Unit.Classes, Image?> g_faceImages = [];
        static Dictionary<string, Image> g_images = [];

        public Unit Unit { get; set; }

        static UnitCard()
        {
            var rm = new ResourceManager(typeof(Properties.Resources));
            g_faceImages[Unit.Classes.None] = rm.GetObject("rookie") as Bitmap;
            g_faceImages[Unit.Classes.Assault] = rm.GetObject("assault") as Bitmap;
            g_faceImages[Unit.Classes.Grenadier] = rm.GetObject("grenadier") as Bitmap;
            g_faceImages[Unit.Classes.Gunner] = rm.GetObject("gunner") as Bitmap;
            g_faceImages[Unit.Classes.Ranger] = rm.GetObject("ranger") as Bitmap;
            g_faceImages[Unit.Classes.Sharpshooter] = rm.GetObject("sharpshooter") as Bitmap;
            g_faceImages[Unit.Classes.Shinobi] = rm.GetObject("shinobi") as Bitmap;
            g_faceImages[Unit.Classes.AdvTropperM1] = rm.GetObject("advent_trooper") as Bitmap;
            g_faceImages[Unit.Classes.AdvCaptainM1] = rm.GetObject("advent_officer") as Bitmap;

            var image = rm.GetObject("attack") as Bitmap;
            if (image != null)
                g_images["[attack]"] = image;
            image = rm.GetObject("health") as Bitmap;
            if (image != null)
                g_images["[health]"] = image;
        }

        public UnitCard(Unit unit_)
        {
            Unit = unit_;
        }

        public override void Draw(Graphics graphics_)
        {
            // face
            if (g_faceImages.ContainsKey(Unit.Class) && g_faceImages[Unit.Class] != null)
            {
                var frameRect = new Rectangle(Point.Empty, g_cardSize);
                Utils.SetCenter(ref frameRect, Location);
                var frameRegion = new Region(frameRect);
                var oldClip = graphics_.Clip;
                graphics_.Clip = frameRegion;
                var image = g_faceImages[Unit.Class];
                var imageSize = image.Size;
                var imageFactor = (float)imageSize.Width / imageSize.Height;
                var cardFactor = (float)g_cardSize.Width / g_cardSize.Height;
                var imageRect = new Rectangle(Point.Empty, g_cardSize);
                if (imageFactor > cardFactor)
                    imageRect.Width = (int)(g_cardSize.Height * imageFactor);
                else
                    imageRect.Height = (int)(g_cardSize.Width / imageFactor);
                Utils.SetCenter(ref imageRect, Location);
                graphics_.DrawImage(image, imageRect, 0, 0, imageSize.Width, imageSize.Height, GraphicsUnit.Pixel);
                graphics_.Clip = oldClip;
            }

            // info
            var rect = new Rectangle(Location.X - g_cardSize.Width / 2, Location.Y + g_cardSize.Height / 2 - 16, g_cardSize.Width, 16);
            var font = new Font(FontFamily.GenericSansSerif, 8);
            var whiteBrush = new SolidBrush(Color.White);
            graphics_.FillRectangle(whiteBrush, rect);
            var blackBrush = new SolidBrush(Color.Black);
            var format = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
            Utils.DrawString(graphics_, Unit.MaxAttack.ToString() + " [attack] " + Unit.Health.ToString() + " [health] ", g_images, font, blackBrush, rect, format);

            base.Draw(graphics_);
        }
    }
}
