using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xcom_tactics
{
    internal class EquipmentInfo
    {
        static List<EquipmentInfo> g_info = [];

        public enum Groups { None, Weapon, UtilityItem };

        public enum Names
        {
            None, AssaultRifle, Cannon, Shotgun, SniperRifle, Sword,
            ArcThrower, SawedOffShotgun, Holotargeter, Gremlin,
            FragGrenage, FlashGrenade, SmokeGrenade,
            CeramicPlating, FirstAidKit
        };

        public Groups Group { get; set; }

        public Names Name { get; set; }

        /*public bool IsGrenade
        {
            get
            {
                return Name == Names.FragGrenage ||
                    Name == Names.FlashGrenade ||
                    Name == Names.SmokeGrenade;
            }
        }*/

        public bool IsPlating
        {
            get
            {
                return Name == Names.CeramicPlating;
            }
        }

        public int Value { get; set; }

        public bool ValueDependsOnAccuracy { get; set; }

        public int AccuracyIncrease { get; set; }

        public Ability? Ability { get; set; }

        static EquipmentInfo()
        {
            g_info.Add(new EquipmentInfo(Names.AssaultRifle, Groups.Weapon) { Value = 3, ValueDependsOnAccuracy = true });
            g_info.Add(new EquipmentInfo(Names.Cannon, Groups.Weapon) { Value = 3, ValueDependsOnAccuracy = true });
            g_info.Add(new EquipmentInfo(Names.Shotgun, Groups.Weapon) { Value = 4, ValueDependsOnAccuracy = true, AccuracyIncrease = 15 });
            g_info.Add(new EquipmentInfo(Names.SniperRifle, Groups.Weapon) { Group = Groups.Weapon, Value = 4, ValueDependsOnAccuracy = true, AccuracyIncrease = 30 });
            g_info.Add(new EquipmentInfo(Names.Sword, Groups.Weapon) { Value = 2 });

            g_info.Add(new EquipmentInfo(Names.FragGrenage, Groups.Weapon) { Value = 3, Ability = new ThrowGrenadeAbility() });
            g_info.Add(new EquipmentInfo(Names.FlashGrenade, Groups.Weapon) { Ability = new ThrowGrenadeAbility() });
            g_info.Add(new EquipmentInfo(Names.SmokeGrenade, Groups.UtilityItem) { Ability = new ThrowGrenadeAbility() });

            g_info.Add(new EquipmentInfo(Names.CeramicPlating,Groups.UtilityItem) { Value = 3 });
            g_info.Add(new EquipmentInfo(Names.FirstAidKit, Groups.UtilityItem) {Value = 2 });
        }

        public static EquipmentInfo Get(Names name_)
        {
            var result = g_info.Find(e => e.Name == name_);
            if (result != null)
                return result;
            return new EquipmentInfo();
        }

        public EquipmentInfo(Names name_ = Names.None, Groups group_ = Groups.None)
        {
            Name = name_;
            Group = group_;
            if (group_ == Groups.Weapon)
                Ability = new AttackAbility();
            else if (group_ == Groups.UtilityItem)
                Ability = new DefenceAbility();
        }
    }

    internal class Equipment : ICloneable
    {
        public EquipmentInfo m_info;

        // боезапас
        // для CeramicPlating - остаток брони
        public int Count { get; set; }

        public Equipment(EquipmentInfo.Names name_, int count_)
        {
            m_info = EquipmentInfo.Get(name_);
            Count = count_;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
