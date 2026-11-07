using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class SlowHitCrystal (RTTI 0x14054aab0, vtable 0x140507270, execute at 0x14012f5e0).
/// Uses 100 ms initial crystal break delay (0x64 at 0x14012f94e) and 200 ms loop delay (0xc8 at 0x14012fa38).
/// </summary>
public sealed class SlowHitCrystalMacro : MacroBase
{
    private const int InitialCrystalBreakDelayMs = 100; // 0x64 at 0x14012f94e
    private const int LoopCrystalPlaceDelayMs = 200;    // 0xc8 at 0x14012fa38

    public SlowHitCrystalMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Slow Hit Crystal", MacroCategory.Crystal, configManager, inputController)
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
        InputController.SleepMillis(InitialCrystalBreakDelayMs);
        InputController.ClickMouse("left");

        while (InputController.IsKeyHeldDown(triggerKeyCode))
        {
            InputController.ClickMouse("right");
            InputController.SleepMillis(LoopCrystalPlaceDelayMs);
            InputController.ClickMouse("left");
            InputController.SleepMillis(delayMs);
        }
    }
}
