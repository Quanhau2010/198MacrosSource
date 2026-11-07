namespace Macros198.Input;

/// <summary>
/// Bidirectional mapper between key names and Windows Virtual Key codes (VK_*).
/// Reconstructed directly from 0x1401291c0 and UTF-16LE tables at 0x140506df0–0x140506e80
/// and 0x14050f1c0–0x14050f2f0.
/// </summary>
public static class KeybindMapper
{
    public const int VK_NONE = -1;
    public const int VK_LBUTTON = 0x01;
    public const int VK_RBUTTON = 0x02;
    public const int VK_MBUTTON = 0x04;
    public const int VK_XBUTTON1 = 0x05; // MOUSE4
    public const int VK_XBUTTON2 = 0x06; // MOUSE5

    public const int VK_BACK = 0x08;
    public const int VK_TAB = 0x09;
    public const int VK_RETURN = 0x0D;
    public const int VK_SHIFT = 0x10;
    public const int VK_CONTROL = 0x11;
    public const int VK_MENU = 0x12; // ALT
    public const int VK_CAPITAL = 0x14; // CAPS
    public const int VK_ESCAPE = 0x1B;
    public const int VK_SPACE = 0x20;
    public const int VK_LEFT = 0x25;
    public const int VK_UP = 0x26;
    public const int VK_RIGHT = 0x27;
    public const int VK_DOWN = 0x28;
    public const int VK_NUMPAD0 = 0x60;
    public const int VK_F1 = 0x70;
    public const int VK_OEM_3 = 0xC0; // GRAVE (`)

    private static readonly Dictionary<string, int> NameToVk = new(StringComparer.OrdinalIgnoreCase);
    private static readonly Dictionary<int, string> VkToName = new();

    static KeybindMapper()
    {
        Register("None", VK_NONE);
        Register("MBUTTON", VK_MBUTTON);
        Register("MIDDLE", VK_MBUTTON);
        Register("MOUSE3", VK_MBUTTON);
        Register("MOUSE4", VK_XBUTTON1);
        Register("MOUSE5", VK_XBUTTON2);

        Register("SPACE", VK_SPACE);
        Register("ENTER", VK_RETURN);
        Register("BACK", VK_BACK);
        Register("ESC", VK_ESCAPE);
        Register("TAB", VK_TAB);
        Register("SHIFT", VK_SHIFT);
        Register("CTRL", VK_CONTROL);
        Register("ALT", VK_MENU);
        Register("CAPS", VK_CAPITAL);
        Register("GRAVE", VK_OEM_3);

        Register("LEFT", VK_LEFT);
        Register("UP", VK_UP);
        Register("RIGHT", VK_RIGHT);
        Register("DOWN", VK_DOWN);

        for (int i = 1; i <= 12; i++)
        {
            Register($"F{i}", VK_F1 + (i - 1));
        }

        for (int i = 0; i <= 9; i++)
        {
            Register($"NUMPAD{i}", VK_NUMPAD0 + i);
        }

        for (char c = '0'; c <= '9'; c++)
        {
            Register(c.ToString(), c);
        }

        for (char c = 'A'; c <= 'Z'; c++)
        {
            Register(c.ToString(), c);
        }
    }

    private static void Register(string name, int vk)
    {
        NameToVk[name] = vk;
        if (!VkToName.ContainsKey(vk))
        {
            VkToName[vk] = name;
        }
    }

    public static int ToVirtualKeyCode(string? keyName)
    {
        if (string.IsNullOrWhiteSpace(keyName))
        {
            return VK_NONE;
        }

        string trimmed = keyName.Trim();
        if (NameToVk.TryGetValue(trimmed, out int vk))
        {
            return vk;
        }

        if (trimmed.Length == 1)
        {
            return char.ToUpperInvariant(trimmed[0]);
        }

        return VK_NONE;
    }

    public static string ToKeyName(int virtualKeyCode) =>
        VkToName.TryGetValue(virtualKeyCode, out string? name) ? name : "None";
}
