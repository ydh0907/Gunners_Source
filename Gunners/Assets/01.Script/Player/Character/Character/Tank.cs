public class Tank : ICharacter
{
    protected void Awake()
    {
        maxHp = 190;
        hp = maxHp;
        armor = 35;
        speed = 4;
    }
}
