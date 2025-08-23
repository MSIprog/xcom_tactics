using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace xcom_tactics
{
    internal class Feature : ICloneable
    {
        public string Name { get; set; }

        public virtual Image? Image { get; }

        public virtual int Value { get; set; }

        public Feature()
        {
            Name = "(NullFeature)";
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    internal class MinMaxFeature : Feature
    {
        int m_value;
        public override int Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
                if (m_value < MinValue)
                    m_value = MinValue;
                if (m_value > MaxValue)
                    m_value = MaxValue;
            }

        }

        int m_minValue;
        public int MinValue
        {
            get
            {
                return m_minValue;
            }
            set
            {
                m_minValue = value;
                m_value = m_minValue;
            }
        }

        int m_maxValue;
        public int MaxValue
        {
            get
            {
                return m_maxValue;
            }
            set
            {
                m_maxValue = value;
                m_value = m_maxValue;
            }
        }
    }

    internal class UnitHealth : MinMaxFeature
    {
        static Image? g_image;

        public override Image? Image
        {
            get
            {
                return g_image;
            }
        }

        static UnitHealth()
        {
            var rm = new ResourceManager(typeof(xcom_tactics.Properties.Resources));
            g_image = rm.GetObject("health") as Bitmap;
        }

        public UnitHealth()
        {
            Name = "Health";
        }
    }

    internal class Accuracy : Feature
    {
        static Image? g_image;

        public override Image? Image
        {
            get
            {
                return g_image;
            }
        }

        static Accuracy()
        {
            var rm = new ResourceManager(typeof(xcom_tactics.Properties.Resources));
            g_image = rm.GetObject("attack") as Bitmap;
        }

        public Accuracy()
        {
            Name = "Accuracy";
        }
    }

    // del if no image
    internal class Damage : Feature
    {
        public Damage()
        {
            Name = "Damage";
        }
    }

    // del if no image
    internal class DamageAbsorption : MinMaxFeature
    {
        public DamageAbsorption()
        {
            Name = "DamageAbsorption";
        }
    }

    // del if no image
    internal class Healing : Feature
    {
        public Healing()
        {
            Name = "Healing";
        }
    }
}
