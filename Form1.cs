namespace xcom_tactics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var enemyUnit1 = new EnemyUnit(Unit.Classes.AdvTropperM1);
            var enemyUnit2 = new EnemyUnit(Unit.Classes.AdvCaptainM1);
            var ourUnit1 = new Unit();
            ourUnit1.AddWeapon(EquipmentInfo.Names.AssaultRifle);
            ourUnit1.AddWeapon(EquipmentInfo.Names.FragGrenage, 2);
            ourUnit1.AddEquipment(EquipmentInfo.Names.FirstAidKit);
            var ourUnit2 = new Unit();
            ourUnit2.AddWeapon(EquipmentInfo.Names.SniperRifle);
            ourUnit2.AddWeapon(EquipmentInfo.Names.FirstAidKit);
            ourUnit2.AddWeapon(EquipmentInfo.Names.FlashGrenade);
            cardSpace.SetUnits([enemyUnit1, enemyUnit2], [ourUnit1, ourUnit2]);

            var outOfPlayUnits = new List<Unit>();
            cardSpace.OnUnitPlaced = delegate (Unit subject_, Unit object_)
            {
                new AttackAbility().UseOnUnit(subject_, subject_.Equipment[0], object_);
                outOfPlayUnits.Add(subject_);
                cardSpace.RemoveUnit(subject_);
                if (object_.GetFeature("Health").Value == 0)
                {
                    outOfPlayUnits.Add(object_);
                    cardSpace.RemoveUnit(object_);
                }
            };
        }
    }
}
