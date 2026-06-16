using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HudButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject selectedHighlight;

    private int index;
    private Action<int> onClicked;

    public void Setup(int index, TurretInventory entry, Action<int> onClicked)
    {
        this.index = index;
        this.onClicked = onClicked;

        if (iconImage != null)
            iconImage.sprite = entry.Icon;

        if (nameText != null)
            nameText.text = entry.TurretPrefab.name;

        SetCount(entry.UsedAmount);

        RegisterToEvents();
    }

    public void SetCount(int count)
    {
        countText.text = count.ToString();
        button.interactable = count > 0;
    }

    private void RegisterToEvents()
    {
        button.onClick.AddListener(Click);
    }

    private void Click()
    {
        onClicked?.Invoke(index);
    }

    public void SetSelected(bool selected)
    {
        if (selectedHighlight != null)
            selectedHighlight.SetActive(selected);
    }

    private void OnDestroy()
    {
        UnregisterFromEvents();
    }

    private void UnregisterFromEvents()
    {
        button.onClick.RemoveListener(Click);
    }
}