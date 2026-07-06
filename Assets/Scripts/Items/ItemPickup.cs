using UnityEngine;

namespace HandOnTheLine.Items
{
    public enum ItemTier { Environmental, Consumable, Secret }

    /// <summary>Generic pickup for Tier2/Tier3 items. Tier1 (environmental) items are interacted
    /// with in place and don't use this component.</summary>
    public class ItemPickup : MonoBehaviour
    {
        [SerializeField] private string itemId;
        [SerializeField] private ItemTier tier = ItemTier.Consumable;

        public string ItemId => itemId;
        public ItemTier Tier => tier;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            Debug.Log($"Picked up {itemId} (Tier: {tier})");
            gameObject.SetActive(false);
        }
    }
}
