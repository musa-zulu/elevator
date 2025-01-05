namespace SharedKernel;
public abstract class Entity
{
    public int Id { get; private init; }

    protected Entity(int id)
    {
        Id = id;
    }

    protected Entity() { }
}
