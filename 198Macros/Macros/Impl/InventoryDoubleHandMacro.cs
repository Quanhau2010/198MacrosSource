using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class InventoryDHand (RTTI 0x14054a9e8, vtable 0x140507108, execute at 0x14012e1e0).
/// </summary>
public sealed class InventoryDoubleHandMacro : MacroBase
{
    public InventoryDoubleHandMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Inventory D-Hand", MacroCategory.Crystal, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int totemSlotKey = GetSlotVirtualKeyCode("Totem Slot");
        InputController.PressKey(totemSlotKey);
    }
}
