using Macros198.Config;
using Macros198.Input;
using Macros198.Macros.Impl;

namespace Macros198.Macros;

/// <summary>
/// Factory and registry of all 14 combat macro implementations, reconstructed in the exact
/// instantiation order of 0x14016df20 (0x14016df74–0x14016e9a4).
/// </summary>
public sealed class MacroRegistry
{
    public IReadOnlyList<MacroBase> Macros { get; }

    public MacroRegistry(ConfigurationManager configManager, InputSimulationController inputController)
    {
        Macros = new List<MacroBase>
        {
            new HitCrystalMacro(configManager, inputController),
            new SingleAnchorMacro(configManager, inputController),
            new DoubleAnchorMacro(configManager, inputController),
            new AnchorPearlMacro(configManager, inputController),
            new SlowHitCrystalMacro(configManager, inputController),
            new KeyPearlMacro(configManager, inputController),
            new InventoryDoubleHandMacro(configManager, inputController),
            new OffhandHotbarTotemMacro(configManager, inputController),
            new FastXpMacro(configManager, inputController),
            new StunSlamMacro(configManager, inputController),
            new PearlCatchMacro(configManager, inputController),
            new ElytraSwapMacro(configManager, inputController),
            new BreachSwapMacro(configManager, inputController),
            new AutoShieldBreakerMacro(configManager, inputController)
        };
    }

    public MacroBase? FindByName(string name) =>
        Macros.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase));
}
