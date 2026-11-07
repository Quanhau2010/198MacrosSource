using System.Runtime.InteropServices;
using Macros198.Config;
using Macros198.Macros;

namespace Macros198.Input;

/// <summary>
/// Global Win32 low-level keyboard and mouse hook manager and asynchronous macro dispatcher.
/// Reconstructed directly from:
/// - 0x14015c7df: SetWindowsHookExW(WH_KEYBOARD_LL = 13, LowLevelKeyboardProc, hInstance, 0)
/// - 0x14015c808: SetWindowsHookExW(WH_MOUSE_LL = 14, LowLevelMouseProc, hInstance, 0)
/// - 0x14012b3c0: LowLevelKeyboardProc with active-key debounce set (0x14012ba30)
/// - 0x14012b320: LowLevelMouseProc dispatching XBUTTON1/2, MBUTTON, LBUTTON, and RBUTTON
/// - 0x14012a290: Key-press macro dispatcher & detached thread spawner (0x14012bd40)
/// - 0x14012acc0: Mouse-click macro dispatcher & detached thread spawner (0x14012bda0)
/// </summary>
public sealed class GlobalInputDispatcher : IDisposable
{
    private readonly ConfigurationManager _configManager;
    private readonly MacroRegistry _macroRegistry;
    private readonly HashSet<int> _activeDebouncedKeys = new();
    private readonly object _debounceLock = new();

    private IntPtr _keyboardHookHandle = IntPtr.Zero;
    private IntPtr _mouseHookHandle = IntPtr.Zero;

    // Keep strong delegate references to prevent GC collection while hooks are active
    private readonly Win32NativeMethods.LowLevelHookProc _keyboardProcDelegate;
    private readonly Win32NativeMethods.LowLevelHookProc _mouseProcDelegate;

    public Action<int>? KeybindCaptureCallback { get; set; }
    public Action? ToggleUiCallback { get; set; }

    public GlobalInputDispatcher(ConfigurationManager configManager, MacroRegistry macroRegistry)
    {
        _configManager = configManager;
        _macroRegistry = macroRegistry;
        _keyboardProcDelegate = LowLevelKeyboardCallback;
        _mouseProcDelegate = LowLevelMouseCallback;
    }

    public void InstallGlobalHooks()
    {
        IntPtr hMod = Win32NativeMethods.GetModuleHandleW(null);
        if (_keyboardHookHandle == IntPtr.Zero)
        {
            _keyboardHookHandle = Win32NativeMethods.SetWindowsHookExW(
                Win32NativeMethods.WH_KEYBOARD_LL,
                _keyboardProcDelegate,
                hMod,
                0);
        }

        if (_mouseHookHandle == IntPtr.Zero)
        {
            _mouseHookHandle = Win32NativeMethods.SetWindowsHookExW(
                Win32NativeMethods.WH_MOUSE_LL,
                _mouseProcDelegate,
                hMod,
                0);
        }
    }

    private IntPtr LowLevelKeyboardCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && lParam != IntPtr.Zero)
        {
            var kbInfo = Marshal.PtrToStructure<Win32NativeMethods.KBDLLHOOKSTRUCT>(lParam);

            // Ignore synthetic keystrokes injected by our own SendInput calls
            if ((kbInfo.flags & Win32NativeMethods.LLKHF_INJECTED) == 0)
            {
                int msg = wParam.ToInt32();
                int vkCode = (int)kbInfo.vkCode;

                // 0x14012b3fe..0x14012b410: (wParam - 0x100) & ~4 == 0 -> WM_KEYDOWN or WM_SYSKEYDOWN
                if (msg == Win32NativeMethods.WM_KEYDOWN || msg == Win32NativeMethods.WM_SYSKEYDOWN)
                {
                    OnKeyDown(vkCode);
                }
                else if (msg == Win32NativeMethods.WM_KEYUP || msg == Win32NativeMethods.WM_SYSKEYUP)
                {
                    OnKeyUp(vkCode);
                }
            }
        }

        return Win32NativeMethods.CallNextHookEx(_keyboardHookHandle, nCode, wParam, lParam);
    }

    private IntPtr LowLevelMouseCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && lParam != IntPtr.Zero)
        {
            var msInfo = Marshal.PtrToStructure<Win32NativeMethods.MSLLHOOKSTRUCT>(lParam);

            // Ignore synthetic mouse clicks injected by our own SendInput calls
            if ((msInfo.flags & Win32NativeMethods.LLMHF_INJECTED) == 0)
            {
                int msg = wParam.ToInt32();
                if (msg == Win32NativeMethods.WM_XBUTTONDOWN)
                {
                    int xBtn = (msInfo.mouseData >> 16) == 1
                        ? KeybindMapper.VK_XBUTTON1
                        : KeybindMapper.VK_XBUTTON2;
                    OnKeyDown(xBtn);
                }
                else if (msg == Win32NativeMethods.WM_XBUTTONUP)
                {
                    int xBtn = (msInfo.mouseData >> 16) == 1
                        ? KeybindMapper.VK_XBUTTON1
                        : KeybindMapper.VK_XBUTTON2;
                    OnKeyUp(xBtn);
                }
                else if (msg == Win32NativeMethods.WM_MBUTTONDOWN)
                {
                    OnKeyDown(KeybindMapper.VK_MBUTTON);
                }
                else if (msg == Win32NativeMethods.WM_MBUTTONUP)
                {
                    OnKeyUp(KeybindMapper.VK_MBUTTON);
                }
                else if (msg == Win32NativeMethods.WM_LBUTTONDOWN)
                {
                    OnMouseClick(1);
                }
                else if (msg == Win32NativeMethods.WM_RBUTTONDOWN)
                {
                    OnMouseClick(2);
                }
            }
        }

        return Win32NativeMethods.CallNextHookEx(_mouseHookHandle, nCode, wParam, lParam);
    }

    public void OnKeyDown(int virtualKeyCode)
    {
        if (virtualKeyCode <= 0)
        {
            return;
        }

        lock (_debounceLock)
        {
            if (!_activeDebouncedKeys.Add(virtualKeyCode))
            {
                return;
            }
        }

        var capture = KeybindCaptureCallback;
        if (capture != null)
        {
            KeybindCaptureCallback = null;
            capture(virtualKeyCode);
            return;
        }

        int toggleVk = KeybindMapper.ToVirtualKeyCode(_configManager.ToggleUiKeybind);
        if (toggleVk != KeybindMapper.VK_NONE && toggleVk == virtualKeyCode)
        {
            ToggleUiCallback?.Invoke();
        }

        foreach (var macro in _macroRegistry.Macros)
        {
            var cfg = macro.GetConfiguration();
            if (cfg == null || !cfg.Enabled)
            {
                continue;
            }

            int boundVk = KeybindMapper.ToVirtualKeyCode(cfg.Keybind);
            if (boundVk != KeybindMapper.VK_NONE && boundVk == virtualKeyCode)
            {
                var targetMacro = macro;
                Task.Run(() => targetMacro.OnKeyPress(virtualKeyCode));
            }
        }
    }

    public void OnKeyUp(int virtualKeyCode)
    {
        lock (_debounceLock)
        {
            _activeDebouncedKeys.Remove(virtualKeyCode);
        }
    }

    public void OnMouseClick(int mouseButton)
    {
        if (KeybindCaptureCallback != null)
        {
            return;
        }

        foreach (var macro in _macroRegistry.Macros)
        {
            var cfg = macro.GetConfiguration();
            if (cfg != null && cfg.Enabled)
            {
                var targetMacro = macro;
                Task.Run(() => targetMacro.OnMouseClick(mouseButton));
            }
        }
    }

    public void Dispose()
    {
        if (_keyboardHookHandle != IntPtr.Zero)
        {
            Win32NativeMethods.UnhookWindowsHookEx(_keyboardHookHandle);
            _keyboardHookHandle = IntPtr.Zero;
        }

        if (_mouseHookHandle != IntPtr.Zero)
        {
            Win32NativeMethods.UnhookWindowsHookEx(_mouseHookHandle);
            _mouseHookHandle = IntPtr.Zero;
        }
    }
}
