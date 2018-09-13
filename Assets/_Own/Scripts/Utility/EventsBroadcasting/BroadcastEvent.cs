using System;
using UnityEngine.Assertions;

public interface IBroadcastEvent
{
    void PostEvent();
    void DeliverEvent();
    MessageDeliveryType deliveryType { get; }
}

public abstract class BroadcastEvent<T> : IBroadcastEvent where T : BroadcastEvent<T>
{
    public delegate void Handler(T eventData);
    public static event Handler handlers;

    private bool wasDeliveryTypeSet;
    public MessageDeliveryType deliveryType { get; private set; }
    
    private bool isPosted;
    private bool isDelivered;

    public void PostEvent()
    {
        Assert.IsFalse(isPosted, $"{this} has already been posted!");
        EventsManager.instance.Post(this);
        isPosted = true;
    }
    
    void IBroadcastEvent.DeliverEvent()
    {
        Assert.IsFalse(isDelivered, $"{this} has already been delivered!");
        handlers?.Invoke((T)this);
        isDelivered = true;
    }
    
    public BroadcastEvent<T> SetDeliveryType(MessageDeliveryType newDeliveryType)
    {
        Assert.IsFalse(wasDeliveryTypeSet, "You can only set the delivery type of a broadcast event once.");
        deliveryType = newDeliveryType;
        wasDeliveryTypeSet = true;

        return this;
    }
}
