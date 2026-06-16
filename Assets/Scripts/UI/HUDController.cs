using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private HudButton buttonPrefab;
    [SerializeField] private Transform buttonHolder;

    private List<HudButton> buttons = new List<HudButton>();

    private void OnEnable()
    {
        RegisterToEvents();

        Refresh();
    }

    private void RegisterToEvents()
    {
        inventory.OnChanged += Refresh;
    }

    private void Refresh()
    {
        if (buttons.Count != inventory.TurretInventory.Count)
            Rebuild();

        for (int i = 0; i < buttons.Count; i++)
        {
            TurretInventory entry = inventory.TurretInventory[i];
            buttons[i].SetCount(entry.UsedAmount);
            buttons[i].SetSelected(i == inventory.SelectedIndex);
        }
    }

    private void Rebuild()
    {
        for (int i = 0; i < buttons.Count; i++)
            Destroy(buttons[i].gameObject);

        buttons.Clear();

        for (int i = 0; i < inventory.TurretInventory.Count; i++)
        {
            HudButton button = Instantiate(buttonPrefab, buttonHolder);
            button.Setup(i, inventory.TurretInventory[i], OnButtonClicked);
            buttons.Add(button);
        }
    }

    private void OnButtonClicked(int index)
    {
        inventory.Select(index);
    }

    private void OnDisable()
    {
        UnregisterFromEvents();
    }

    private void OnDestroy()
    {
        UnregisterFromEvents();
    }

    private void UnregisterFromEvents()
    {
        inventory.OnChanged -= Refresh;
    }
}