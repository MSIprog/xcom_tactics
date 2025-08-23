using System;

namespace xcom_tactics
{
    internal class Unit : ICloneable
    {
        public enum Classes { None, Assault, Grenadier, Gunner, Ranger, Sharpshooter, Shinobi,
            AdvTropperM1, AdvCaptainM1, AdvGunnerM1, AdvSentryM1, AdvGrenadierM1
        };

        public enum Sides { None, Xcom, Advent }

        // для картинки на карте
        // todo: depends on weapon
        public Classes Class { get; set; }

        // точность и запас здоровья (текущий и максимальный)
        List<Feature> m_features = [];

        public Sides Side { get; set; }

        List<Equipment> m_equipment = [];
        public List<Equipment> Equipment
        {
            get
            {
                return m_equipment;
            }
        }

        // для надписи на карте
        public int MaxAttack
        {
            get
            {
                return m_equipment.Max(e => e.m_info.Value);
            }
        }

        List<Effect> m_effects = [];
        public List<Effect> Effects => m_effects;

        public Unit()
        {
            m_features.Add(new UnitHealth { MaxValue = 3 });
            m_features.Add(new Accuracy { Value = 75 });
        }

        public void TakeEffect(Effect effect_)
        {
            m_effects.RemoveAll(e => e.m_info.Name == effect_.m_info.Name);
            m_effects.Add(effect_);
        }

        public void RemoveEffect(EffectInfo.Names name_)
        {
            m_effects.RemoveAll(e => e.m_info.Name == name_);
        }

        public List<Ability> GetAbilities()
        {
            List<Ability> result = [];
            foreach (var equipment in m_equipment)
                if (equipment.m_info.Ability != null)
                    result.Add(equipment.m_info.Ability);
            return result;
        }

        public Feature GetFeature(string name_)
        {
            var result = m_features.Find(f => f.Name == name_);
            if (result == null)
                throw new Exception("Feature not found");
            return result;
        }

        public MinMaxFeature GetMinMaxFeature(string name_)
        {
            var feature = m_features.Find(f => f.Name == name_);
            if (feature == null)
                throw new Exception("Feature not found");
            var result = feature as MinMaxFeature;
            if (result == null)
                throw new Exception("Feature is not MinMaxFeature");
            return result;
        }

        public void ModifyAction(Action action_, bool thisIsSubject_)
        {
            if (thisIsSubject_)
            {
                if (action_.Features.Any(f => f.Name == "Accuracy"))
                    action_.AddFeatureValue("Accuracy", GetFeature("Accuracy").Value, "[unit] ");
            }
        }

        /*public void OnAction(Action action_, bool thisIsSubject_)
        {
            // attack action
            if (thisIsSubject_)
            {
                action_.GetFeature("Accuracy").Value += GetFeature("Accuracy").Value;

                // current weapon
                var equipment = m_equipment[action_.GetFeature("Equipment").Value];
                equipment.OnAction(action_, true);

                Effects.ForEach(e => e.OnAction(action_));
            }
            else
            {
                // effects

                // armor, plating
                m_equipment.ForEach(e => e.OnAction(action_, false));

                // body
                GetFeature("Health").Value -= action_.GetFeature("Damage").Value;
            }
        }*/

        public object Clone()
        {
            var result = MemberwiseClone() as Unit;
            result.m_equipment = new List<Equipment>();
            foreach (var equipment in m_equipment)
                result.m_equipment.Add(equipment.Clone() as Equipment);
            /*result.Abilities = new List<Ability>();
            foreach (var ability in Abilities)
                result.Abilities.Add(ability.Clone() as Ability);*/
            result.m_effects = new List<Effect>();
            foreach (var effect in m_effects)
                result.m_effects.Add(effect.Clone() as Effect);
            return result;
        }
    }
}
