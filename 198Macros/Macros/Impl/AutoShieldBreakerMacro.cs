using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class AutoShieldBreaker (RTTI 0x14054a928, vtable 0x140506fc8, execute at 0x14012d010).
/// Decrypted inline stack strings: "Axe Slot" (movabs 0x746f6c5320657841 at 0x14012d0c6) and "left" (0x7466656c at 0x14012d294).
/// </summary>
public sealed class AutoShieldBreakerMacro : MacroBase
{
    public AutoShieldBreakerMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Auto Shield Breaker", MacroCategory.Sword, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int delayMs = GetConfiguredDelayMs();
        int axeSlotKey = GetSlotVirtualKeyCode("Axe Slot");
        int swordSlotKey = GetSlotVirtualKeyCode("Sword Slot");

        InputController.PressKey(axeSlotKey);
        InputController.ClickMouse("left");
        InputController.SleepMillis(delayMs);
        InputController.PressKey(swordSlotKey);
    }
}
