using System.Windows.Forms;
using Macros198.Config;
using Macros198.Input;
using Macros198.Macros;
using Macros198.UI;

namespace Macros198;

/// <summary>
/// Clean, deobfuscated C# entry point reconstructed from WinMain at 0x14015c6b0:
/// - Eradicates VMAware::detect (0x14014ab60) and 25 anti-debug checks (0x14015d540).
/// - Eradicates HWID collection (0x1401133d0) and .vlizer cloud auth stubs (0x14017b7c0, 0x14017c2f0).
/// - Initializes all 15 themes (0x14014d4c0), 14 macro cards (0x14014f1e0), local JSON config (0x140114da0),
///   and global Win32 WH_KEYBOARD_LL / WH_MOUSE_LL hooks (0x14015c7df, 0x14015c808).
/// </summary>
internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var configManager = new ConfigurationManager();
        try
        {
            configManager.LoadFromFile();
        }
        catch
        {
            // Ignore malformed or missing initial config and use built-in defaults
        }

        var inputController = new InputSimulationController();
        var macroRegistry = new MacroRegistry(configManager, inputController);
        using var inputDispatcher = new GlobalInputDispatcher(configManager, macroRegistry);

        inputDispatcher.InstallGlobalHooks();

        using var dashboardForm = new MacroDashboardForm(configManager, macroRegistry, inputDispatcher);
        Application.Run(dashboardForm);
    }
}
