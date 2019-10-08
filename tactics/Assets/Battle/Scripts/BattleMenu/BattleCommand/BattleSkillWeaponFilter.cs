public class BattleSkillWeaponFilter : BattleSkillFilter
{
    public bool this[BattleAgent agent, Skill skill]
    {
        get
        {
            string tag1 = (agent.BaseCharacter.PrimaryWeapon == null ? WeaponType.Fist : agent.BaseCharacter.PrimaryWeapon.Type).ToString();
            string tag2 = (agent.BaseCharacter.SecondaryWeapon == null ? WeaponType.Fist : agent.BaseCharacter.SecondaryWeapon.Type).ToString();

            return skill.Tags.Contains("Weapon") && (skill.Tags.Contains(tag1) || skill.Tags.Contains(tag2));
        }
    }
}
