
using UnityEngine;

/* The base item class. All items should derive from this. */

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{

    new public string name = "Sapling";    // Name of the item
    public Sprite icon = null;              // Item icon
    public bool isDefaultItem = true;      // Is the item default wear?

    // Called when the item is pressed in the inventory
    public virtual void Use()
    {
        // Use the item
        // Something might happen

        Debug.Log("Właśnie postawiłes sadzonke " + name);
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }

}