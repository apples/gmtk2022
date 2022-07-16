
public interface ICharacterBehavior
{
    void BeginTurn();
    TurnResult PerformTurn();
}

public abstract class CharacterBehavior : ICharacterBehavior
{
    public TurnTaker Controller { get; set; }

    public abstract void BeginTurn();
    public abstract TurnResult PerformTurn();
}
