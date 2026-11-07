using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class OffhandHotbarTotem (RTTI 0x14054aa30, vtable 0x140507190, execute at 0x14012e770).
/// </summary>
public sealed class OffhandHotbarTotemMacro : MacroBase
{
    public OffhandHotbarTotemMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Offhand Hotbar Totem", MacroCategory.Crystal, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int totemSlotKey = GetSlotVirtualKeyCode("Totem Slot");
        int swapHandsKey = GetSlotVirtualKeyCode("Swap Hands Bind");

        InputController.PressKey(totemSlotKey);
        InputController.SleepMillis(delayMs);
        InputController.PressKey(swapHandsKey);
    }
}
