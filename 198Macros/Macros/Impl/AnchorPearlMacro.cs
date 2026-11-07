using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class AnchorPearl (RTTI 0x14054a8e0, vtable 0x140506f58, execute at 0x14012c940).
/// </summary>
public sealed class AnchorPearlMacro : MacroBase
{
    private const int PearlFollowupDelayMs = 50; // 0x32 at 0x14012ce54

    public AnchorPearlMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Anchor Pearl", MacroCategory.Crystal, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int anchorSlotKey = GetSlotVirtualKeyCode("Anchor Slot");
        int glowstoneSlotKey = GetSlotVirtualKeyCode("Glowstone Slot");
        int pearlSlotKey = GetSlotVirtualKeyCode("Pearl Slot");

        InputController.PressKey(anchorSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);

        InputController.PressKey(glowstoneSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);

        InputController.PressKey(pearlSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(PearlFollowupDelayMs);
        InputController.ClickMouse("right");
    }
}
