using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class ElytraSwap (RTTI 0x14054a9a0, vtable 0x140507070, execute at 0x14012dd70).
/// </summary>
public sealed class ElytraSwapMacro : MacroBase
{
    public ElytraSwapMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Elytra Swap", MacroCategory.Mace, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int elytraSlotKey = GetSlotVirtualKeyCode("Elytra/Chestplate Slot");

        InputController.PressKey(elytraSlotKey);
        InputController.SleepMillis(delayMs);
        InputController.ClickMouse("right");
    }
}
