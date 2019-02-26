namespace Data
{
    public interface ITransceiver<T> : IReceiver<T>, ITransmitter<T>
    {
    }
}