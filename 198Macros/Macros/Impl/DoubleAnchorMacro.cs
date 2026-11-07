using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class DoubleAnchor (RTTI 0x14054a978, vtable 0x140507050, execute at 0x14012d610).
/// </summary>
public sealed class DoubleAnchorMacro : MacroBase
{
    public DoubleAnchorMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Double Anchor", MacroCategory.Crystal, configManager, inputController)
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

        InputController.PressKey(anchorSlotKey);
        InputController.ClickMouse("right");
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
