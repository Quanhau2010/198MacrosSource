using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class BreachSwap (RTTI 0x14054a950, vtable 0x140507008, onMouseClick at 0x14012d400).
/// Overrides vtable[2] (OnMouseClick): when mouseButton == 1 (Left Click), swaps to Mace Slot,
/// sleeps delayMs, and swaps back to Sword Slot.
/// </summary>
public sealed class BreachSwapMacro : MacroBase
{
    private const int LeftMouseButton = 1;

    public BreachSwapMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Breach Swap", MacroCategory.Mace, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode) => OnMouseClick(LeftMouseButton);

    public override void OnMouseClick(int mouseButton)
    {
        if (mouseButton != LeftMouseButton)
        {
            return;
        }

        int delayMs = GetConfiguredDelayMs();
        int maceSlotKey = GetSlotVirtualKeyCode("Mace Slot");
        int swordSlotKey = GetSlotVirtualKeyCode("Sword Slot");

        InputController.PressKey(maceSlotKey);
        InputController.SleepMillis(delayMs);
        InputController.PressKey(swordSlotKey);
    }
}
