namespace Macros198.Models;

/// <summary>
/// Combat macro categories reconstructed from the three global vector tables in
/// 0x14014f1e0 (0x14054f6f8, 0x14054f710, 0x14054f6c8) and UTF-16LE sidebar labels
/// at 0x14050ee38–0x14050f160.
/// </summary>
public enum MacroCategory
{
    Crystal = 0,
    Mace = 1,
    Sword = 2
}

public static class MacroCategoryExtensions
{
    public static string GetShortLabel(this MacroCategory category) => category switch
    {
        MacroCategory.Crystal => "Crystal",
        MacroCategory.Mace => "Mace",
        MacroCategory.Sword => "Sword",
        _ => "Crystal"
    };

    public static string GetHeaderTitle(this MacroCategory category) => category switch
    {
        MacroCategory.Crystal => "Crystal Macros",
        MacroCategory.Mace => "Mace Macros",
        MacroCategory.Sword => "Sword Macros",
        _ => "Crystal Macros"
    };

    public static string GetSubtitle(this MacroCategory category) => category switch
    {
        MacroCategory.Crystal => "Configure your end crystal PVP automation",
        MacroCategory.Mace => "Configure your mace combat macros",
        MacroCategory.Sword => "Configure your sword combat macros",
        _ => ""
    };

    public static string GetIconIdentifier(this MacroCategory category) => category switch
    {
        MacroCategory.Crystal => "crystal-icon",
        MacroCategory.Mace => "mace-icon",
        MacroCategory.Sword => "sword-icon",
        _ => "crystal-icon"
    };
}
