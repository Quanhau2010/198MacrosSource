using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class HitCrystal (RTTI 0x14054ae68, vtable 0x14050fa18, execute at 0x14016d930).
/// </summary>
public sealed class HitCrystalMacro : MacroBase
{
    public HitCrystalMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Hit Crystal", MacroCategory.Crystal, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int obsidianSlotKey = GetSlotVirtualKeyCode("Obsidian Slot");
        int crystalSlotKey = GetSlotVirtualKeyCode("Crystal Slot");

        InputController.PressKey(obsidianSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);

        InputController.PressKey(crystalSlotKey);
        InputController.ClickMouse("right");
        InputController.SleepMillis(delayMs);

        InputController.ClickMouse("left");

        while (InputController.IsKeyHeldDown(triggerKeyCode))
        {
            InputController.ClickMouse("right");
            InputController.SleepMillis(delayMs);
            InputController.ClickMouse("left");
            InputController.SleepMillis(delayMs);
        }
    }
}
