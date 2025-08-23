using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace xcom_tactics
{
    internal abstract class Ability
    {
        public virtual bool CanUseOnUnit(Unit subject_, Equipment equipment_, Unit object_)
        {
            return false;
        }

        public abstract Action CreateAction(Unit subject_, Equipment equipment_, Unit object_);

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

        public override Action CreateAction(Unit subject_, Equipment equipment_, Unit object_)
        {
            var action = new Action(subject_, object_);
            action.Features.Add(new Feature { Name = "Accuracy" });
            action.Features.Add(new Damage());

            // outcoming damage calc
            subject_.ModifyAction(action, true);
            /*action.AddFeatureValue("Accuracy", subject_.GetFeature("Accuracy").Value, "[unit] ");
            action.AddFeatureValue("Accuracy", equipment_.GetFeature("Accuracy").Value, "[weapon] ");
            action.GetFeature("Damage").Value = equipment_.GetFeature("Damage").Value;*/
            equipment_.ModifyAction(action, true);

            // incoming damage calc
            //object_.OnAction(action, false);
            object_.Equipment.ForEach(e => e.ModifyAction(action, false));
            equipment_.ModifyAction(action, true);

            // execute
            /*action.GetFeature("Damage").Value = action.GetFeature("Damage").Value * action.GetFeature("Accuracy").Value / 100;
            action.Features.RemoveAll(f => f.Name == "Accuracy");
            object_.Equipment.ForEach(e => e.ExecuteAction(action, false));
            object_.GetFeature("Health").Value -= action.GetFeature("Damage").Value;*/
            //TakeEffect

            return action;
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

        public override Action CreateAction(Unit subject_, Equipment equipment_, Unit object_)
        {
            var action = new Action(subject_, object_);
            return action;
        }
    }

    internal class Action
    {
        // todo: feature container
        List<Feature> m_features = [];
        public List<Feature> Features
        {
            get
            {
                return m_features;
            }
        }

        /*public int Damage
        {
            get
            {
                if (Features.Any(f => f.Name == "Damage"))
                {
                    if (Features.Any(f => f.Name == "Accuracy"))
                        return GetFeature("Damage").Value * GetFeature("Accuracy").Value / 100;
                    else
                        return GetFeature("Damage").Value;
                }
                return 0;
            }
        }*/

        public Unit SubjectUnit { get; set; }

        public Unit ObjectUnit { get; set; }

        public string Text { get; set; }

        public Action(Unit subject_, Unit object_)
        {
            SubjectUnit = subject_;
            ObjectUnit = object_;
            Text = "";
        }

        public Feature GetFeature(string name_)
        {
            var result = m_features.Find(f => f.Name == name_);
            if (result == null)
                throw new Exception("Feature not found");
            return result;
        }

        public void AddFeatureValue(string feature_, int value_, string commentPrefix_)
        {
            if (value_ == 0)
                return;
            GetFeature(feature_).Value += value_;
            Text += commentPrefix_ + (value_ < 0 ? "-" : "+") + value_;
        }

        public void Execute()
        {
            GetFeature("Damage").Value = GetFeature("Damage").Value * GetFeature("Accuracy").Value / 100;
            Features.RemoveAll(f => f.Name == "Accuracy");
            ObjectUnit.Equipment.ForEach(e => e.ExecuteAction(this, false));
            ObjectUnit.GetFeature("Health").Value -= GetFeature("Damage").Value;
        }
    }
}
