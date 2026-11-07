using Macros198.Models;

namespace Macros198.Config;

/// <summary>
/// Runtime configuration and UI metadata for a single macro card,
/// reconstructed from the 168-byte (0xA8) struct initialized at 0x140150a30.
/// </summary>
public sealed class MacroConfiguration
{
    public string Name { get; }
    public string Description { get; }
    public string BadgeCode { get; }
    public MacroCategory Category { get; }
    public bool Enabled { get; set; }
    public string Keybind { get; set; }
    public int DelayMs { get; set; }
    public Dictionary<string, string> Slots { get; }

    public MacroConfiguration(
        string name,
        string description,
        string badgeCode,
        MacroCategory category,
        int defaultDelayMs,
        IDictionary<string, string> defaultSlots)
    {
        Name = name;
        Description = description;
        BadgeCode = badgeCode;
        Category = category;
        Enabled = false;
        Keybind = "None";
        DelayMs = defaultDelayMs;
        Slots = new Dictionary<string, string>(defaultSlots, StringComparer.OrdinalIgnoreCase);
    }

    public string GetSlotValue(string slotName) =>
        Slots.TryGetValue(slotName, out string? val) && val != null ? val : string.Empty;

    public void SetSlotValue(string slotName, string value) =>
        Slots[slotName] = (value ?? string.Empty).Trim();
}
