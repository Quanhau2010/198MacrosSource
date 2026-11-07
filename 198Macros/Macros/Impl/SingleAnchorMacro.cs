using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class SingleAnchor (RTTI 0x14054aa88, vtable 0x140507218, execute at 0x14012ef80).
/// </summary>
public sealed class SingleAnchorMacro : MacroBase
{
    public SingleAnchorMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Single Anchor", MacroCategory.Crystal, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int anchorSlotKey = GetSlotVirtualKeyCode("Anchor Slot");
        int glowstoneSlotKey = GetSlotVirtualKeyCode("Glowstone Slot");
        int totemExplodeSlotKey = GetSlotVirtualKeyCode("Totem/Explode Slot");

        InputController.PressKey(anchorSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);

        InputController.PressKey(glowstoneSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);

        InputController.PressKey(totemExplodeSlotKey);
        InputController.ClickMouse("right");
    }
}
