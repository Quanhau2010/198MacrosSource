using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class KeyPearl (RTTI 0x14054aa10, vtable 0x140507148, execute at 0x14012e360).
/// </summary>
public sealed class KeyPearlMacro : MacroBase
{
    public KeyPearlMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Key Pearl", MacroCategory.Crystal, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int pearlSlotKey = GetSlotVirtualKeyCode("Pearl Slot");
        int swapBackSlotKey = GetSlotVirtualKeyCode("Swap Back Slot");

        InputController.PressKey(pearlSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);
        InputController.PressKey(swapBackSlotKey);
    }
}
