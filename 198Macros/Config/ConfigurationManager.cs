using System.Text.Json;
using System.Text.Json.Nodes;
using Macros198.Models;

namespace Macros198.Config;

/// <summary>
/// Manages macro configuration state and local JSON persistence.
/// Reconstructed from 0x1401142a0 (saveConfig), 0x140114da0 (loadConfig),
/// 0x14014bae0 (config map sync at 0x14054f530), and 0x14014f1e0 (default card initialization).
/// Cloud endpoints (/saveConfig, /getConfig) and Code Virtualizer wrappers (0x140178760, 0x140178de0)
/// have been completely eradicated in favor of local JSON storage.
/// </summary>
public sealed class ConfigurationManager
{
    private readonly Dictionary<string, MacroConfiguration> _macroConfigs =
        new(StringComparer.OrdinalIgnoreCase);

    public string ConfigFilePath { get; }
    public ThemePalette ActiveTheme { get; set; } = ThemePalette.All[0];
    public string ToggleUiKeybind { get; set; } = "None";

    public ConfigurationManager(string? customConfigPath = null)
    {
        ConfigFilePath = customConfigPath ?? Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".198macros",
            "config.json");
        InitializeDefaultCards();
    }

    public IReadOnlyCollection<MacroConfiguration> GetAllMacroConfigs() => _macroConfigs.Values;

    public MacroConfiguration? GetMacroConfig(string macroName) =>
        _macroConfigs.TryGetValue(macroName, out var cfg) ? cfg : null;

    /// <summary>
    /// Populates all 14 macro cards with the exact defaults from 0x14014f1e0–0x140150a30.
    /// </summary>
    public void InitializeDefaultCards()
    {
        _macroConfigs.Clear();

        // Category 1: Crystal Macros (0x14054f6f8)
        RegisterCard(
            "Hit Crystal",
            "Auto place obsidian and hit crystal",
            "HC",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string> { ["Crystal Slot"] = "2", ["Obsidian Slot"] = "3" });

        RegisterCard(
            "Single Anchor",
            "Auto place and explode an anchor",
            "SA",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string>
            {
                ["Anchor Slot"] = "5",
                ["Glowstone Slot"] = "6",
                ["Totem/Explode Slot"] = "8"
            });

        RegisterCard(
            "Double Anchor",
            "Auto place and explode two anchors",
            "DA",
            MacroCategory.Crystal,
            75,
            new Dictionary<string, string>
            {
                ["Anchor Slot"] = "5",
                ["Glowstone Slot"] = "6",
                ["Totem/Explode Slot"] = "8"
            });

        RegisterCard(
            "Anchor Pearl",
            "Anchor and pearl right after to get low ground first",
            "AP",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string>
            {
                ["Anchor Slot"] = "5",
                ["Glowstone Slot"] = "6",
                ["Pearl Slot"] = "8"
            });

        RegisterCard(
            "Slow Hit Crystal",
            "Hit Crystal with optimal timing",
            "SHC",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string> { ["Crystal Slot"] = "2", ["Obsidian Slot"] = "3" });

        RegisterCard(
            "Key Pearl",
            "Switches and throws pearl, then switches back",
            "KP",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string> { ["Pearl Slot"] = "2", ["Swap Back Slot"] = "3" });

        RegisterCard(
            "Inventory D-Hand",
            "Presses totem slot when opening inventory.
 ! Keybind this to your open inventory key. !",
            "IDH",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string> { ["Totem Slot"] = "8" });

        RegisterCard(
            "Offhand Hotbar Totem",
            "Offhands your hotbar totem when pressed",
            "OHT",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string> { ["Totem Slot"] = "8", ["Swap Hands Bind"] = "F" });

        RegisterCard(
            "Fast XP",
            "Automatically spams right click to exp faster",
            "FXP",
            MacroCategory.Crystal,
            50,
            new Dictionary<string, string>());

        // Category 2: Mace Macros (0x14054f710)
        RegisterCard(
            "Stun Slam",
            "Auto break shields with mace",
            "SS",
            MacroCategory.Mace,
            80,
            new Dictionary<string, string> { ["Axe slot"] = "8", ["Mace Slot"] = "7" });

        RegisterCard(
            "Pearl Catch",
            "Automatically go up using a pearl and wind charge",
            "PC",
            MacroCategory.Mace,
            50,
            new Dictionary<string, string> { ["Pearl slot"] = "4", ["Wind Charge Slot"] = "2" });

        RegisterCard(
            "Elytra Swap",
            "Swaps to your elytra/chestplate slot and presses right click",
            "ES",
            MacroCategory.Mace,
            50,
            new Dictionary<string, string> { ["Elytra/Chestplate Slot"] = "2" });

        RegisterCard(
            "Breach Swap",
            "Automatically swaps to mace when swinging with your sword",
            "BS",
            MacroCategory.Mace,
            50,
            new Dictionary<string, string> { ["Mace Slot"] = "5", ["Sword Slot"] = "1" });

        // Category 3: Sword Macros (0x14054f6c8)
        RegisterCard(
            "Auto Shield Breaker",
            "Automatically uses axe to break shields",
            "ASB",
            MacroCategory.Sword,
            50,
            new Dictionary<string, string> { ["Axe Slot"] = "8", ["Sword Slot"] = "1" });
    }

    private void RegisterCard(
        string name,
        string description,
        string badgeCode,
        MacroCategory category,
        int defaultDelayMs,
        IDictionary<string, string> defaultSlots)
    {
        _macroConfigs[name] = new MacroConfiguration(
            name, description, badgeCode, category, defaultDelayMs, defaultSlots);
    }

    /// <summary>
    /// Serializes configuration to the exact JSON schema produced by 0x1401142a0.
    /// </summary>
    public string SerializeToJson()
    {
        var root = new JsonObject();
        foreach (var card in _macroConfigs.Values)
        {
            var slotsNode = new JsonObject();
            foreach (var kv in card.Slots)
            {
                slotsNode[kv.Key] = kv.Value;
            }

            root[card.Name] = new JsonObject
            {
                ["enabled"] = card.Enabled,
                ["keybind"] = card.Keybind,
                ["delay"] = card.DelayMs,
                ["slots"] = slotsNode
            };
        }

        root["theme"] = ActiveTheme.Index;
        root["toggleUI"] = ToggleUiKeybind;

        return root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    }

    /// <summary>
    /// Loads configuration from the JSON schema parsed by 0x140114da0.
    /// </summary>
    public void LoadFromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        var root = JsonNode.Parse(json) as JsonObject;
        if (root == null)
        {
            return;
        }

        if (root["theme"] is JsonValue themeVal && themeVal.TryGetValue<int>(out int themeIndex))
        {
            ActiveTheme = ThemePalette.FromIndex(themeIndex);
        }

        if (root["toggleUI"] is JsonValue toggleVal && toggleVal.TryGetValue<string>(out string? toggleStr) && !string.IsNullOrWhiteSpace(toggleStr))
        {
            ToggleUiKeybind = toggleStr;
        }

        foreach (var card in _macroConfigs.Values)
        {
            if (root[card.Name] is not JsonObject macroNode)
            {
                continue;
            }

            if (macroNode["enabled"] is JsonValue enVal && enVal.TryGetValue<bool>(out bool enabled))
            {
                card.Enabled = enabled;
            }

            if (macroNode["keybind"] is JsonValue kbVal && kbVal.TryGetValue<string>(out string? keybind) && !string.IsNullOrWhiteSpace(keybind))
            {
                card.Keybind = keybind;
            }

            if (macroNode["delay"] is JsonValue dlVal && dlVal.TryGetValue<int>(out int delay))
            {
                card.DelayMs = Math.Max(0, delay);
            }

            if (macroNode["slots"] is JsonObject slotsObj)
            {
                foreach (var prop in slotsObj)
                {
                    if (prop.Value is JsonValue sVal && sVal.TryGetValue<string>(out string? slotBind) && slotBind != null)
                    {
                        card.SetSlotValue(prop.Key, slotBind);
                    }
                }
            }
        }
    }

    public void SaveToFile()
    {
        string? dir = Path.GetDirectoryName(ConfigFilePath);
        if (!string.IsNullOrEmpty(dir))
        {
            Directory.CreateDirectory(dir);
        }
        File.WriteAllText(ConfigFilePath, SerializeToJson());
    }

    public void LoadFromFile()
    {
        if (File.Exists(ConfigFilePath))
        {
            LoadFromJson(File.ReadAllText(ConfigFilePath));
        }
    }
}
