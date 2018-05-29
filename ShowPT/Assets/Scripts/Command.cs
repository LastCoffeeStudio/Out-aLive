public abstract class Command
{
    public abstract void execute();
    public virtual void move() {}
    public virtual void shoot() {}
    public virtual void interact() {}
    public virtual void crouch() {}
    public virtual void aim() {}
    public virtual void pause() {}
    public virtual void select() {}
}
