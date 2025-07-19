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

        // точность
        public int Accuracy { get; set; }

        // todo: не использовать эффекты!
        // макимальный запас здоровья;
        // фактический = максимальный - урон;
        // урон хранится в эффектах
        public int MaxHealth { get; set; }

        public int Health
        {
            get
            {
                var result = MaxHealth;
                var injuredEffect = m_effects.Find(e => e.m_info.Name == EffectInfo.Names.Injured);
                if (injuredEffect != null)
                    result -= injuredEffect.Value;
                return result;
            }
            set
            {
                var injuredValue = MaxHealth - value;
                if (injuredValue == 0)
                    RemoveEffect(EffectInfo.Names.Injured);
                else
                    TakeEffect(new Effect(EffectInfo.Names.Injured) { Value = injuredValue });
            }
        }

        public Sides Side { get; set; }

        List<Equipment> m_equipment = [];

        // для надписи на карте
        public int MaxAttack
        {
            get
            {
                return m_equipment.Max(e => GetAttack(e, null));
            }
        }

        List<Effect> m_effects = [];

        public int AccuracyDecrease
        {
            get
            {
                return m_effects.Max(e => e.m_info.AccuracyDecrease);
            }
        }

        public Unit()
        {
            Accuracy = 75;
            MaxHealth = 3;
        }

        public void AddEquipment(EquipmentInfo.Names name_, int count_ = 1)
        {
            m_equipment.Add(new Equipment(name_, count_));
        }

        public int GetAttack(Equipment equipment_, Unit? object_)
        {
            if (equipment_.m_info.Group != EquipmentInfo.Groups.Weapon)
                return 0;
            int accuracyDecrease = object_ != null ? object_.AccuracyDecrease : 0;
            return equipment_.m_info.ValueDependsOnAccuracy ?
                equipment_.m_info.Value * (Accuracy + equipment_.m_info.AccuracyIncrease - accuracyDecrease) / 100 :
                equipment_.m_info.Value;
        }

        public void TakeDamage(int value_)
        {
            // effects

            // plating
            var plating = m_equipment.Find(e => e.m_info.IsPlating);
            if (plating != null)
            {
                var oldValue = value_;
                value_ -= plating.Count;
                plating.Count -= oldValue;
                if (plating.Count < 0)
                    plating.Count = 0;
            }

            // armor

            // body
            Health -= value_;
        }

        public void Heal(int value_)
        {
            Health += value_;
        }

        public void TakeEffect(Effect effect_)
        {
            // урон и лечение работают так же
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
