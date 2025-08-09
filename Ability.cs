using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace xcom_tactics
{
    internal class Ability
    {
        public virtual bool CanUseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            return false;
        }

        public virtual void UseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
        }

        public virtual Image? GetImage()
        {
            return null;
        }
    }

    internal class AttackAbility : Ability
    {
        static Image? g_image;

        static AttackAbility()
        {
            var rm = new ResourceManager(typeof(xcom_tactics.Properties.Resources));
            g_image = rm.GetObject("attack_ability") as Bitmap;
        }

        public override bool CanUseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            if (equipment_.m_info.Group != EquipmentInfo.Groups.Weapon)
                return false;
            if (subject_.Side == object_.Side)
                return false;

            // ...

            return true;
        }

        public override void UseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            var action = new Action();
            action.Features.Add(new Feature { Name = "Accuracy" });
            action.Features.Add(new Damage());

            // outcoming damage
            //subject_.OnAction(action, true);
            action.GetFeature("Accuracy").Value = subject_.GetFeature("Accuracy").Value;
            equipment_.OnAction(action, true);

            // incoming damage
            //object_.OnAction(action, false);
            object_.Equipment.ForEach(e => e.OnAction(action, false));
            object_.GetFeature("Health").Value -= action.GetFeature("Damage").Value;
            //TakeEffect
        }

        public override Image? GetImage()
        {
            return g_image;
        }
    }

    internal class ThrowGrenadeAbility : AttackAbility
    {
        static Image? g_image;

        static ThrowGrenadeAbility()
        {
            var rm = new ResourceManager(typeof(xcom_tactics.Properties.Resources));
            g_image = rm.GetObject("attack_grenade") as Bitmap;
        }

        public override bool CanUseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            if (!base.CanUseOnUnit(subject_, equipment_, object_))
                return false;
            if (equipment_.GetFeature("Quantity").Value == 0)
                return false;
            return true;
        }

        public override void UseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            --equipment_.GetFeature("Quantity").Value;
            base.UseOnUnit(subject_, equipment_, object_);
        }
    }

    internal class DefenceAbility : Ability
    {
        public override bool CanUseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            if (equipment_.m_info.Group != EquipmentInfo.Groups.UtilityItem)
                return false;
            if (subject_.Side != object_.Side)
                return false;
            if (equipment_.GetFeature("Quantity").Value == 0)
                return false;
            return true;
        }

        public override void UseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            --equipment_.GetFeature("Quantity").Value;
            if (equipment_.m_info.Value != 0)
                object_.GetFeature("Health").Value += equipment_.m_info.Value;
            //TakeEffect
        }
    }

    internal class Action
    {
        List<Feature> m_features = [];
        public List<Feature> Features
        {
            get
            {
                return m_features;
            }
        }

        public Feature GetFeature(string name_)
        {
            var result = m_features.Find(f => f.Name == name_);
            if (result == null)
                throw new Exception("Feature not found");
            return result;
        }
    }
}
