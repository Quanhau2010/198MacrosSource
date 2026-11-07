using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class StunSlam (RTTI 0x14054aad8, vtable 0x1405072a0, execute at 0x14012fbd0).
/// Decrypted inline stack string: "Axe slot" (movabs 0x746f6c7320657841 at 0x14012fc8d).
/// </summary>
public sealed class StunSlamMacro : MacroBase
{
    public StunSlamMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Stun Slam", MacroCategory.Mace, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int axeSlotKey = GetSlotVirtualKeyCode("Axe slot");
        int maceSlotKey = GetSlotVirtualKeyCode("Mace Slot");

        InputController.PressKey(axeSlotKey);
        InputController.ClickMouse("left");
        InputController.SleepMillis(delayMs);
        InputController.PressKey(maceSlotKey);

        while (InputController.IsKeyHeldDown(triggerKeyCode))
        {
            InputController.ClickMouse("left");
            InputController.SleepMillis(delayMs);
        }
    }
}
