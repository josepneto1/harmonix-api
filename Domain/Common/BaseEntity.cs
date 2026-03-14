namespace Harmonix.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public bool Removed { get; private set; } = false;
    public DateTimeOffset CreatedAt { get; protected set; }
    public DateTimeOffset? UpdatedAt { get; protected set; }

    protected virtual Guid GenerateNewId() => Guid.CreateVersion7();

    public void SetCreated() => CreatedAt = DateTimeOffset.UtcNow;

    public void SetUpdated() => UpdatedAt = DateTimeOffset.UtcNow;

    public void Remove() => Removed = true;

    public void Restore() => Removed = false;
}
