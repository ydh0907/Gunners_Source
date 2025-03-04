public class Scout : ICharacter
{
    private void Awake()
    {
        maxHp = 100;
        hp = maxHp;
        armor = 10;
        speed = 10;
    }
}
