/*
 * ICanBeInteracted INTERFACE: This is an interface to be used on objects
 * that are interactables via MOUSE or KEYBOARD.
 */
public interface ICanBeInteracted<T>
{
    void PickableInteraction(T _whichHand);
    void MouseClickInteraction();
}
