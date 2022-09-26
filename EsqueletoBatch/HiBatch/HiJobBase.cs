namespace EsqueletoBatch.HiBatch;
public class HiJobBase
{
    protected readonly Guid _guid;
    public HiJobBase()
    {
        _guid = Guid.NewGuid();
    }
}