using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class PearlCatch (RTTI 0x14054aa60, vtable 0x1405071e8, execute at 0x14012eb00).
/// </summary>
public sealed class PearlCatchMacro : MacroBase
{
    public PearlCatchMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Pearl Catch", MacroCategory.Mace, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int pearlSlotKey = GetSlotVirtualKeyCode("Pearl slot");
        int windChargeSlotKey = GetSlotVirtualKeyCode("Wind Charge Slot");

        InputController.PressKey(pearlSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);
        InputController.PressKey(windChargeSlotKey);
        InputController.ClickMouse("right");
    }
}
