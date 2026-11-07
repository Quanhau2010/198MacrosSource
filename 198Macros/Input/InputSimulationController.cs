using System.Runtime.InteropServices;

namespace Macros198.Input;

/// <summary>
/// Thread-safe keyboard and mouse input synthesis controller.
/// Reconstructed byte-for-byte from:
/// - 0x14017cb90: Mutex-locked SendInput(2, inputs, 0x28) for key / mouse-button press & release
/// - 0x14017ccc0: Mutex-locked SendInput(2, inputs, 0x28) for mouse click ("left", "middle", "right")
/// - 0x1403e9410: KERNEL32!Sleep delay primitive
/// - 0x1403e9810: USER32!GetAsyncKeyState(vk) < 0 key-hold state check
/// </summary>
public sealed class InputSimulationController
{
    private readonly object _inputMutex = new();

    /// <summary>
    /// Replicates 0x14017cb90: acquires mutex (0x140549d20), builds a 2-element INPUT[2] array
    /// (40 bytes per element on x64), and dispatches USER32.dll!SendInput.
    /// </summary>
    public void PressKey(int virtualKeyCode)
    {
        if (virtualKeyCode <= 0)
        {
            return;
        }

        lock (_inputMutex)
        {
            var inputs = new Win32NativeMethods.INPUT[2];

            if (virtualKeyCode == KeybindMapper.VK_MBUTTON)
            {
                // 0x14017cbea..0x14017cbff: VK_MBUTTON (4) -> MIDDLEDOWN (0x20) / MIDDLEUP (0x40)
                inputs[0].type = Win32NativeMethods.INPUT_MOUSE;
                inputs[0].U.mi.dwFlags = Win32NativeMethods.MOUSEEVENTF_MIDDLEDOWN;

                inputs[1].type = Win32NativeMethods.INPUT_MOUSE;
                inputs[1].U.mi.dwFlags = Win32NativeMethods.MOUSEEVENTF_MIDDLEUP;
            }
            else if (virtualKeyCode == KeybindMapper.VK_XBUTTON1 || virtualKeyCode == KeybindMapper.VK_XBUTTON2)
            {
                // 0x14017cc01..0x14017cc57: VK_XBUTTON1 (5) / VK_XBUTTON2 (6) -> XDOWN (0x80) / XUP (0x100)
                uint xButtonData = (virtualKeyCode == KeybindMapper.VK_XBUTTON1) ? 1u : 2u;

                inputs[0].type = Win32NativeMethods.INPUT_MOUSE;
                inputs[0].U.mi.dwFlags = Win32NativeMethods.MOUSEEVENTF_XDOWN;
                inputs[0].U.mi.mouseData = xButtonData;

                inputs[1].type = Win32NativeMethods.INPUT_MOUSE;
                inputs[1].U.mi.dwFlags = Win32NativeMethods.MOUSEEVENTF_XUP;
                inputs[1].U.mi.mouseData = xButtonData;
            }
            else
            {
                // 0x14017cc09..0x14017cc33: INPUT_KEYBOARD (1), wVk = virtualKeyCode, down (0) then KEYUP (2)
                inputs[0].type = Win32NativeMethods.INPUT_KEYBOARD;
                inputs[0].U.ki.wVk = (ushort)virtualKeyCode;
                inputs[0].U.ki.dwFlags = 0;

                inputs[1].type = Win32NativeMethods.INPUT_KEYBOARD;
                inputs[1].U.ki.wVk = (ushort)virtualKeyCode;
                inputs[1].U.ki.dwFlags = Win32NativeMethods.KEYEVENTF_KEYUP;
            }

            Win32NativeMethods.SendInput(2, inputs, Marshal.SizeOf<Win32NativeMethods.INPUT>());
        }
    }

    /// <summary>
    /// Replicates 0x14017ccc0: lowercases buttonName ("left", "middle", "right"), builds INPUT[2],
    /// and dispatches USER32.dll!SendInput under the shared mutex.
    /// </summary>
    public void ClickMouse(string buttonName)
    {
        if (string.IsNullOrWhiteSpace(buttonName))
        {
            return;
        }

        string norm = buttonName.Trim().ToLowerInvariant();
        uint downFlag;
        uint upFlag;

        if (norm == "left")
        {
            downFlag = Win32NativeMethods.MOUSEEVENTF_LEFTDOWN;
            upFlag = Win32NativeMethods.MOUSEEVENTF_LEFTUP;
        }
        else if (norm == "middle")
        {
            downFlag = Win32NativeMethods.MOUSEEVENTF_MIDDLEDOWN;
            upFlag = Win32NativeMethods.MOUSEEVENTF_MIDDLEUP;
        }
        else if (norm == "right")
        {
            downFlag = Win32NativeMethods.MOUSEEVENTF_RIGHTDOWN;
            upFlag = Win32NativeMethods.MOUSEEVENTF_RIGHTUP;
        }
        else
        {
            return;
        }

        lock (_inputMutex)
        {
            var inputs = new Win32NativeMethods.INPUT[2];
            inputs[0].type = Win32NativeMethods.INPUT_MOUSE;
            inputs[0].U.mi.dwFlags = downFlag;

            inputs[1].type = Win32NativeMethods.INPUT_MOUSE;
            inputs[1].U.mi.dwFlags = upFlag;

            Win32NativeMethods.SendInput(2, inputs, Marshal.SizeOf<Win32NativeMethods.INPUT>());
        }
    }

    /// <summary>
    /// Replicates test reg, reg; jle skip; call KERNEL32!Sleep (0x1403e9410).
    /// </summary>
    public void SleepMillis(int milliseconds)
    {
        if (milliseconds > 0)
        {
            Thread.Sleep(milliseconds);
        }
    }

    /// <summary>
    /// Replicates call USER32!GetAsyncKeyState (0x1403e9810); test ax, ax; js loop.
    /// </summary>
    public bool IsKeyHeldDown(int virtualKeyCode)
    {
        if (virtualKeyCode <= 0)
        {
            return false;
        }
        return (Win32NativeMethods.GetAsyncKeyState(virtualKeyCode) & 0x8000) != 0;
    }
}
