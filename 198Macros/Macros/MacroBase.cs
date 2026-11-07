using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros;

/// <summary>
/// Base class for all 14 combat macros in 198Macros v1.4.0.
/// Reconstructed from class Macro (RTTI TypeDescriptor 0x14054a908, vtable 0x140506f78):
/// - 0x14012c190: Virtual scalar deleting destructor
/// - 0x14012c1d0: GetSettingString(macroName, settingKey)
/// - 0x14012c6f0: GetDelaySetting(macroName, "delay")
/// - 0x14012c890: GetSlotVirtualKeyCode(macroName, slotName)
/// - 0x140118d90: Default no-op virtual handler
/// </summary>
public abstract class MacroBase
{
    public string Name { get; }
    public MacroCategory Category { get; }
    protected ConfigurationManager ConfigManager { get; }
    protected InputSimulationController InputController { get; }

    protected MacroBase(
        string name,
        MacroCategory category,
        ConfigurationManager configManager,
        InputSimulationController inputController)
    {
        Name = name;
        Category = category;
        ConfigManager = configManager;
        InputController = inputController;
    }

    public MacroConfiguration? GetConfiguration() => ConfigManager.GetMacroConfig(Name);

    protected int GetConfiguredDelayMs() => GetConfiguration()?.DelayMs ?? 0;

    protected int GetSlotVirtualKeyCode(string slotName)
    {
        var cfg = GetConfiguration();
        if (cfg == null)
        {
            return KeybindMapper.VK_NONE;
        }
        return KeybindMapper.ToVirtualKeyCode(cfg.GetSlotValue(slotName));
    }

    /// <summary>
    /// Virtual method slot 1 (vtable[1]): Invoked when the macro's keybind is pressed.
    /// </summary>
    public virtual void OnKeyPress(int triggerKeyCode)
    {
    }

    /// <summary>
    /// Virtual method slot 2 (vtable[2]): Invoked when a mouse button is clicked (1 = Left, 2 = Right).
    /// </summary>
    public virtual void OnMouseClick(int mouseButton)
    {
    }
}
