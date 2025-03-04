public class Soldier : ICharacter
{
    private void Awake()
    {
        maxHp = 150;
        hp = maxHp;
        armor = 25;
        speed = 7;
    }
}
