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
            if (equipment_.Count == 0)
                return false;

            // ...

            return true;
        }

        public override void UseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            --equipment_.Count;
            object_.TakeDamage(subject_.GetAttack(equipment_, object_));
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
    }

    internal class DefenceAbility : Ability
    {
        public override bool CanUseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            if (equipment_.m_info.Group != EquipmentInfo.Groups.UtilityItem)
                return false;
            if (subject_.Side != object_.Side)
                return false;
            if (equipment_.Count == 0)
                return false;
            return true;
        }

        public override void UseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            --equipment_.Count;
            if (equipment_.m_info.Value != 0)
                object_.Heal(equipment_.m_info.Value);
            //TakeEffect
        }
    }
}
