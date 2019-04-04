public interface  IObservable
{
    void NotifyChange();
    void AddListener(IListener listener);
    void RemoveListener(IListener listener);

}
