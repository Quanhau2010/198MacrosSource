using System.Drawing;

namespace Macros198.Models;

/// <summary>
/// UI theme palettes reconstructed from 0x14014d4c0 and UTF-16LE literals at 0x14050def8–0x14050e2a0.
/// </summary>
public sealed record ThemePalette(
    int Index,
    string Name,
    string Description,
    Color BackgroundColor,
    Color CardColor,
    Color AccentColor,
    Color TextColor,
    Color MutedTextColor)
{
    public static readonly IReadOnlyList<ThemePalette> All = new List<ThemePalette>
    {
        new(0, "Dark", "Classic dark theme", Color.FromArgb(14, 14, 18), Color.FromArgb(25, 25, 30), Color.FromArgb(99, 102, 241), Color.FromArgb(240, 240, 245), Color.FromArgb(153, 153, 165)),
        new(1, "Dark Blue", "Premium cyan accents", Color.FromArgb(10, 15, 24), Color.FromArgb(17, 23, 36), Color.FromArgb(34, 211, 238), Color.FromArgb(240, 243, 250), Color.FromArgb(155, 165, 185)),
        new(2, "Dark Purple", "Royal violet accents", Color.FromArgb(17, 11, 24), Color.FromArgb(25, 17, 36), Color.FromArgb(173, 85, 247), Color.FromArgb(243, 240, 250), Color.FromArgb(173, 155, 190)),
        new(3, "Dark Green", "Matrix emerald vibes", Color.FromArgb(9, 20, 15), Color.FromArgb(15, 30, 23), Color.FromArgb(34, 224, 120), Color.FromArgb(240, 249, 244), Color.FromArgb(155, 192, 170)),
        new(4, "Dark Red", "Crimson blood accent", Color.FromArgb(24, 11, 13), Color.FromArgb(35, 17, 20), Color.FromArgb(234, 68, 68), Color.FromArgb(249, 240, 240), Color.FromArgb(192, 155, 155)),
        new(5, "Midnight", "Deep space darkness", Color.FromArgb(7, 8, 14), Color.FromArgb(12, 14, 24), Color.FromArgb(100, 127, 245), Color.FromArgb(224, 228, 240), Color.FromArgb(141, 149, 170)),
        new(6, "Dark Orange", "Warm amber glow", Color.FromArgb(24, 16, 11), Color.FromArgb(35, 24, 17), Color.FromArgb(255, 149, 38), Color.FromArgb(249, 245, 240), Color.FromArgb(202, 179, 155)),
        new(7, "Dark Pink", "Hot magenta vibes", Color.FromArgb(24, 11, 18), Color.FromArgb(35, 17, 28), Color.FromArgb(255, 81, 168), Color.FromArgb(249, 240, 245), Color.FromArgb(202, 155, 180)),
        new(8, "Dracula", "Legendary vampire theme", Color.FromArgb(35, 37, 48), Color.FromArgb(50, 52, 68), Color.FromArgb(189, 147, 249), Color.FromArgb(241, 250, 245), Color.FromArgb(189, 195, 210)),
        new(9, "Arctic", "Pristine frozen white", Color.FromArgb(28, 31, 38), Color.FromArgb(39, 43, 52), Color.FromArgb(132, 215, 255), Color.FromArgb(244, 249, 255), Color.FromArgb(176, 189, 205)),
        new(10, "Obsidian", "Premium black & gold", Color.FromArgb(8, 8, 10), Color.FromArgb(17, 17, 22), Color.FromArgb(255, 215, 64), Color.FromArgb(250, 250, 245), Color.FromArgb(188, 188, 175)),
        new(11, "Slate", "Modern minimalist gray", Color.FromArgb(15, 17, 22), Color.FromArgb(26, 29, 36), Color.FromArgb(137, 161, 196), Color.FromArgb(244, 246, 250), Color.FromArgb(181, 189, 200)),
        new(12, "Espresso", "Rich coffee warmth", Color.FromArgb(19, 15, 13), Color.FromArgb(33, 26, 22), Color.FromArgb(217, 165, 102), Color.FromArgb(246, 243, 238), Color.FromArgb(197, 184, 170)),
        new(13, "Tokyo Night", "Neon city streets", Color.FromArgb(24, 26, 35), Color.FromArgb(37, 41, 55), Color.FromArgb(119, 137, 255), Color.FromArgb(201, 220, 250), Color.FromArgb(144, 166, 200)),
        new(14, "Abyss", "Deep ocean depths", Color.FromArgb(5, 10, 17), Color.FromArgb(11, 19, 30), Color.FromArgb(0, 204, 255), Color.FromArgb(224, 244, 255), Color.FromArgb(148, 186, 210))
    };

    public static ThemePalette FromIndex(int index) =>
        All.FirstOrDefault(t => t.Index == index) ?? All[0];

    public override string ToString() => $"{Name} — {Description}";
}
