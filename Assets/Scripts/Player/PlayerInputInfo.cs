public struct PlayerInputInfo
{
    public PlayerInputInfo(float horizontalVal, float verticalVal, bool attackVal)
    {
        horizontal = horizontalVal;
        vertical = verticalVal;
        attack = attackVal;
    }

    public float horizontal;
    public float vertical;
    public bool attack;
}
