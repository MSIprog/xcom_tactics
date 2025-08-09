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

        // todo: del
        public int Value { get; set; }

        // todo: del
        public int AccuracyIncrease { get; set; }

        // todo: list
        public Ability? Ability { get; set; }

        static EquipmentInfo()
        {
            g_info.Add(new EquipmentInfo(Names.AssaultRifle, Groups.Weapon) { Value = 3 });
            g_info.Add(new EquipmentInfo(Names.Cannon, Groups.Weapon) { Value = 3 });
            g_info.Add(new EquipmentInfo(Names.Shotgun, Groups.Weapon) { Value = 4, AccuracyIncrease = 15 });
            g_info.Add(new EquipmentInfo(Names.SniperRifle, Groups.Weapon) { Group = Groups.Weapon, Value = 4, AccuracyIncrease = 30 });
            g_info.Add(new EquipmentInfo(Names.Sword, Groups.Weapon) { Value = 2 });

            g_info.Add(new EquipmentInfo(Names.FragGrenage, Groups.Weapon) { Value = 3, Ability = new ThrowGrenadeAbility() });
            g_info.Add(new EquipmentInfo(Names.FlashGrenade, Groups.Weapon) { Ability = new ThrowGrenadeAbility() });
            g_info.Add(new EquipmentInfo(Names.SmokeGrenade, Groups.UtilityItem) { Ability = new ThrowGrenadeAbility() });

            g_info.Add(new EquipmentInfo(Names.CeramicPlating,Groups.UtilityItem) { Value = 3 });
            g_info.Add(new EquipmentInfo(Names.FirstAidKit, Groups.UtilityItem) { Value = 2 });
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

        // todo: List<EquipmentModification> m_slots;

        List<Feature> m_features = [];
        public List<Feature> Features
        {
            get
            {
                return m_features;
            }
        }

        public Equipment(EquipmentInfo.Names name_, int quantity_ = 1)
        {
            m_info = EquipmentInfo.Get(name_);
            m_features.Add(new MinMaxFeature { Name = "Quantity", MaxValue = quantity_ });
        }

        public Feature GetFeature(string name_)
        {
            var result = m_features.Find(f => f.Name == name_);
            if (result == null)
                throw new Exception("Feature not found");
            return result;
        }

        public virtual void OnAction(Action action_, bool thisIsSubject_)
        {
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    internal class Weapon : Equipment
    {
        public Weapon(EquipmentInfo.Names name_, int quantity_ = 1) : base(name_, quantity_)
        {
            Features.Add(new Damage { Value = m_info.Value });
        }

        public override void OnAction(Action action_, bool thisIsSubject_)
        {
            if (thisIsSubject_)
                action_.GetFeature("Damage").Value += m_info.Value * (action_.GetFeature("Accuracy").Value + m_info.AccuracyIncrease) / 100;
        }
    }

    internal class Armor : Equipment
    {
        public Armor(EquipmentInfo.Names name_, int armor_) : base(name_)
        {
            Features.Add(new DamageAbsorption { MaxValue = armor_ });
        }

        public override void OnAction(Action action_, bool thisIsSubject_)
        {
            if (!thisIsSubject_)
            {
                var damage = action_.GetFeature("Damage").Value;
                var oldValue = damage;
                damage -= GetFeature("DamageAbsorption").Value;
                if (damage < 0)
                    damage = 0;
                action_.GetFeature("Damage").Value = damage;
            }
        }
    }

    internal class Plating : Equipment
    {
        public Plating(EquipmentInfo.Names name_, int plating_) : base(name_)
        {
            Features.Add(new DamageAbsorption { MaxValue = plating_ });
        }

        public override void OnAction(Action action_, bool thisIsSubject_)
        {
            if (!thisIsSubject_)
            {
                var damage = action_.GetFeature("Damage").Value;
                var plating = GetFeature("DamageAbsorption").Value;
                var oldValue = damage;
                damage -= plating;
                if (damage < 0)
                    damage = 0;
                action_.GetFeature("Damage").Value = damage;
                plating -= oldValue;
                if (plating < 0)
                    plating = 0;
                GetFeature("DamageAbsorption").Value = plating;
            }
        }
    }

    internal class FirstAidKit : Equipment
    {
        public FirstAidKit(EquipmentInfo.Names name_, int healing_) : base(name_)
        {
            Features.Add(new Healing { Value = healing_ });
            Features.Add(new MinMaxFeature { Name = "Quantity", MaxValue = 2 });
        }
    }
}
