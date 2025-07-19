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
                    Accuracy = 65;
                    MaxHealth = 4;
                    AddEquipment(EquipmentInfo.Names.AssaultRifle);
                    break;
                case Classes.AdvCaptainM1:
                    Accuracy = 85;
                    MaxHealth = 7;
                    AddEquipment(EquipmentInfo.Names.AssaultRifle);
                    AddEquipment(EquipmentInfo.Names.FragGrenage, 2);
                    break;
            }
        }
    }
}
