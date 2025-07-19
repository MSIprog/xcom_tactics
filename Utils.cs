using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static xcom_tactics.Utils;

namespace xcom_tactics
{
    public static class Utils
    {
        public struct Cortege2<T1, T2>
        {
            public T1 m_1;

            public T2 m_2;

            public Cortege2(T1 m1_, T2 m2_)
            {
                m_1 = m1_;
                m_2 = m2_;
            }

            public override string ToString()
            {
                return (m_1 != null ? m_1.ToString() : "null") + ", " + (m_2 != null ? m_2.ToString() : "null");
            }
        }

        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds_, int radius_)
        {
            int diameter = radius_ * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds_.Location, size);
            GraphicsPath path = new GraphicsPath();
            // top left arc  
            path.AddArc(arc, 180, 90);
            // top right arc  
            arc.X = bounds_.Right - diameter;
            path.AddArc(arc, 270, 90);
            // bottom right arc  
            arc.Y = bounds_.Bottom - diameter;
            path.AddArc(arc, 0, 90);
            // bottom left arc 
            arc.X = bounds_.Left;
            path.AddArc(arc, 90, 90);
            path.CloseFigure();
            return path;
        }

        public static void SetCenter(ref Rectangle rect_, Point center_)
        {
            rect_.X = center_.X - rect_.Width / 2;
            rect_.Y = center_.Y - rect_.Height / 2;
        }

        public static void SetCenter(ref Rectangle rect_, int centerX_, int centerY_)
        {
            rect_.X = centerX_ - rect_.Width / 2;
            rect_.Y = centerY_ - rect_.Height / 2;
        }

        // value[key]=value
        public static bool ParseIniFileLine(string line_, out string name_, out string key_, out string value_)
        {
            name_ = string.Empty;
            key_ = string.Empty;
            value_ = string.Empty;
            var separatorIndex = line_.IndexOf('=');
            if (separatorIndex == -1)
                return false;
            name_ = line_.Substring(0, separatorIndex);
            var bracketIndex = name_.IndexOf('[');
            if (bracketIndex != -1 && name_[name_.Length - 1] == ']')
            {
                key_ = name_.Substring(bracketIndex + 1, name_.Length - bracketIndex - 2);
                name_ = name_.Substring(0, bracketIndex);
            }
            value_ = line_.Substring(separatorIndex + 1);
            return true;
        }

        public static string GetTimeString(int hours_)
        {
            int daysLeft = hours_ / 24;
            int hoursLeft = hours_ % 24;
            return (daysLeft != 0 ? daysLeft.ToString() + " day(s) " : "") + hoursLeft.ToString() + " hour(s)";
        }

        public static void DrawString(Graphics g_, string s_, Dictionary<string, Image> images_, Font font_, Brush brush_, RectangleF layoutRectangle_, StringFormat format_)
        {
            var words = s_.Split([' ']);
            var points = new PointF[words.Length];
            var lineWidths = new List<float>();
            var lineIndex = 0;
            var wordsInLines = new List<List<int>>
            {
                new List<int>()
            };

            // calc
            float x = 0;
            float y = 0;
            for (var i = 0; i < words.Length; i++)
            {
                var wordSize = new SizeF();
                if (images_.ContainsKey(words[i]))
                {
                    var image = images_[words[i]];
                    wordSize.Width = font_.GetHeight() / image.Height * image.Width;
                    wordSize.Height = font_.GetHeight();
                }
                else
                    wordSize = g_.MeasureString(words[i], font_);
                if (x + wordSize.Width > layoutRectangle_.Width && x != 0)
                {
                    lineWidths.Add(layoutRectangle_.Width - x);
                    x = 0;
                    y += font_.Height;
                    wordsInLines.Add(new List<int>());
                    lineIndex++;
                }
                points[i] = new PointF(x, y);
                x += wordSize.Width;
                wordsInLines[lineIndex].Add(i);
            }
            lineWidths.Add(layoutRectangle_.Width - x);

            // align
            var deltaY = layoutRectangle_.Height - lineWidths.Count * font_.Height;
            if (format_.LineAlignment == StringAlignment.Center)
                deltaY /= 2;
            if (format_.LineAlignment == StringAlignment.Near)
                deltaY = 0;
            for (var li = 0; li < lineWidths.Count; li++)
            {
                var deltaX = lineWidths[li];
                if (format_.Alignment == StringAlignment.Center)
                    deltaX /= 2;
                if (format_.Alignment == StringAlignment.Near)
                    deltaX = 0;
                for (var wi = 0; wi < wordsInLines[li].Count; wi++)
                {
                    points[wordsInLines[li][wi]].X += deltaX;
                    points[wordsInLines[li][wi]].Y += deltaY;
                }
            }

            // draw
            for (var i = 0; i < words.Length; i++)
            {
                if (images_.ContainsKey(words[i]))
                {
                    var image = images_[words[i]];
                    g_.DrawImage(image, new RectangleF(layoutRectangle_.X + points[i].X, layoutRectangle_.Y + points[i].Y, font_.GetHeight() / image.Height * image.Width, font_.GetHeight()));
                }
                else
                    g_.DrawString(words[i], font_, brush_, layoutRectangle_.X + points[i].X, layoutRectangle_.Y + points[i].Y);
            }
        }
    }
}
