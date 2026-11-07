using Macros198.Config;
using Macros198.Input;
using Macros198.Models;

namespace Macros198.Macros.Impl;

/// <summary>
/// Reconstructed from class FastXP (RTTI 0x14054a9c8, vtable 0x1405070c0, execute at 0x14012e040).
/// Spams right-click while triggerKeyCode is held, adding random jitter in [-5, +5] ms (0x14012e14e).
/// </summary>
public sealed class FastXpMacro : MacroBase
{
    public FastXpMacro(ConfigurationManager configManager, InputSimulationController inputController)
        : base("Fast XP", MacroCategory.Crystal, configManager, inputController)
    {
    }

    public override void OnKeyPress(int triggerKeyCode)
    {
        int baseDelayMs = GetConfiguredDelayMs();
        do
        {
            InputController.ClickMouse("right");
            int jitterMs = Random.Shared.Next(-5, 6); // (rand() % 11) - 5 at 0x14012e14e
            int effectiveDelayMs = baseDelayMs + jitterMs;
            if (effectiveDelayMs > 0)
            {
                InputController.SleepMillis(effectiveDelayMs);
            }
        }
        while (InputController.IsKeyHeldDown(triggerKeyCode));
    }
}
