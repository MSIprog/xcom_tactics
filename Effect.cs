using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xcom_tactics
{
    internal class EffectInfo
    {
        static List<EffectInfo> g_info = [];

        public enum Groups { None, Buffs, Debuffs };

        public enum Names { Injured, InSmoke }

        public Groups Group { get; set; }

        public Names Name { get; set; }

        //public int DurationInTurns { get; set; }

        public int AccuracyDecrease { get; set; }

        static EffectInfo()
        {
            g_info.Add(new EffectInfo { Group = Groups.Debuffs, Name = Names.Injured });
            g_info.Add(new EffectInfo { Group = Groups.Buffs, Name = Names.InSmoke, AccuracyDecrease = 30 });
        }

        public static EffectInfo Get(Names name_)
        {
            var result = g_info.Find(e => e.Name == name_);
            if (result != null)
                return result;
            return new EffectInfo();
        }
    }

    internal class Effect
    {
        public EffectInfo m_info;

        public int DurationInTurns { get; set; }

        // для Injured
        public int Value { get; set; }

        public Effect(EffectInfo.Names name_)
        {
            m_info = EffectInfo.Get(name_);
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
