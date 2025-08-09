using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xcom_tactics
{
    internal class EnemyUnit : Unit
    {
        public EnemyUnit(Classes class_)
        {
            Class = class_;
            switch(class_)
            {
                case Classes.AdvTropperM1:
                    GetFeature("Accuracy").Value = 65;
                    GetMinMaxFeature("Health").MaxValue = 4;
                    AddWeapon(EquipmentInfo.Names.AssaultRifle);
                    break;
                case Classes.AdvCaptainM1:
                    GetFeature("Accuracy").Value = 85;
                    GetMinMaxFeature("Health").MaxValue = 7;
                    AddWeapon(EquipmentInfo.Names.AssaultRifle);
                    AddWeapon(EquipmentInfo.Names.FragGrenage);
                    break;
            }
        }
    }
}
