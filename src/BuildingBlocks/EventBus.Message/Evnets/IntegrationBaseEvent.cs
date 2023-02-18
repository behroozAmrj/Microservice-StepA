namespace EventBus.Message.Evnets;

public class IntegrationBaseEvent
{
    public IntegrationBaseEvent()
    {
        Id = Guid.NewGuid();
        CreationDate= DateTime.Now;
    }

    public IntegrationBaseEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }

    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
}
