# 198 Macros v1.4.0 (Purified Edition) — Build & User Guide

A 100% deobfuscated, purified, and native-reconstructed edition of **198 Macros v1.4.0**. All virtualization wrappers (Oreans Code Virtualizer `.vlizer`), hypervisor checks (`VMAware`), anti-debugging traps (`NtQueryInformationProcess`), HWID fingerprinting, and cloud authentication gates have been completely removed while preserving the exact behavior, timing, and input sequences of all 14 Minecraft PvP combat macros.

## 1. System Requirements

* **Operating System**: Windows 10 / Windows 11 (64-bit).
* **C# Native Win32 Edition (`198Macros-CSharp-Project.zip`) — Recommended**:
  * **.NET 8.0 SDK** (for building from source) or **.NET 8.0 Desktop Runtime (x64)** (for running).
  * Zero external dependencies; invokes `user32.dll` (`SetWindowsHookExW`, `GetAsyncKeyState`, `SendInput`) directly via P/Invoke.
* **Java 17 Gradle Edition (`198Macros-Gradle-Project.zip`)**:
  * **JDK 17** or newer.

## 2. Building & Running from Source

### Option A: C# Edition (.NET 8.0 Native Win32)

1. Extract `198Macros-CSharp-Project.zip`.
2. Open a terminal (PowerShell or Command Prompt) inside the `198Macros-CSharp` directory:
   * **Run directly**:
     ```powershell
     dotnet run --project 198Macros/198Macros.csproj -c Release
     ```
   * **Publish a lightweight framework-dependent `.exe`**:
     ```powershell
     dotnet publish 198Macros/198Macros.csproj -c Release -r win-x64 --self-contained false
     ```
     Output executable: `198Macros/bin/Release/net8.0-windows/win-x64/publish/198Macros.exe`.
   * **Publish a standalone single-file `.exe` (no .NET installation required on target PC)**:
     ```powershell
     dotnet publish 198Macros/198Macros.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
     ```
3. Alternatively, open `198Macros.sln` in **Visual Studio 2022**, select **Release | x64**, and press **Build Solution (`Ctrl + Shift + B`)**.

### Option B: Java 17 Gradle Edition

1. Extract `198Macros-Gradle-Project.zip`.
2. Open a terminal inside `198macros-reconstructed` and build the fat JAR:
   ```powershell
   ./gradlew.bat shadowJar
   ```
3. Launch the generated archive:
   ```powershell
   java -jar build/libs/198macros-purified-1.4.0-all.jar
   ```

> **Important Note on Administrator Privileges**:
> If your Minecraft client (Lunar Client, Feather, Badlion, Prism, or official Launcher) runs with **Administrator** privileges, right-click `198Macros.exe` and select **Run as administrator**. Windows UIPI (User Interface Privilege Isolation) blocks non-elevated processes from capturing global hooks (`SetWindowsHookExW`) or injecting `SendInput` events into elevated game windows.

## 3. Dashboard Configuration & Controls

The application opens directly into the main dashboard with four navigation tabs on the left sidebar: **Crystal**, **Mace**, **Sword**, and **Themes & Settings**.

1. **Enabling / Disabling a Macro (`Enabled`)**:
   * Toggle the **Enabled** checkbox in the top-right corner of any macro card to activate or deactivate it.
2. **Binding a Trigger Key (`Keybind`)**:
   * Click the button next to **Keybind:** (defaults to `None`). The button label changes to `Press key...`.
   * Press any keyboard key or mouse button (`MOUSE4`, `MOUSE5`, or middle mouse button `MBUTTON`) to bind it.
   * Press **`ESC`** while `Press key...` is active to clear the keybind back to `None`.
3. **Adjusting Action Delay (`Delay (ms)`)**:
   * Set the delay in milliseconds (`ms`) between hotbar slot switches and mouse clicks.
   * The built-in defaults (`50 ms` – `80 ms`) match the original binary; increase to `60 ms` – `90 ms` on higher-latency servers if slot swaps register too quickly.
4. **Configuring Hotbar Slots (`Slots`)**:
   * Enter the key corresponding to your in-game Minecraft hotbar slot (`1`–`9`, or custom letters such as `R`, `C`, `V`, `F`) into each slot box on the macro card.
5. **UI Visibility Hotkey (`Toggle UI Keybind`) & Themes**:
   * Open the **Themes & Settings** tab on the left sidebar.
   * Choose from all **15 built-in color palettes** (`Dark`, `Dark Blue`, `Dark Purple`, `Dark Green`, `Dark Red`, `Midnight`, `Dark Orange`, `Dark Pink`, `Dracula`, `Arctic`, `Obsidian`, `Slate`, `Espresso`, `Tokyo Night`, `Abyss`).
   * Click **Toggle UI Keybind** to assign a global hotkey (e.g., `INSERT` or `F8`) that hides or restores the macro window while playing.
6. **Configuration Persistence**:
   * Settings are automatically saved to `%USERPROFILE%\.198macros\config.json` (and can also be saved manually via the **Save Config** button in the sidebar).

## 4. Complete Reference for All 14 Macros

### Category 1: Crystal Macros (9 Macros)

| Macro Name | Badge | Default Delay | Default Slot Bindings | Execution Sequence & Hold Behavior |
| :--- | :--- | :--- | :--- | :--- |
| **Hit Crystal** | `HC` | `50 ms` | `Crystal Slot: 2`, `Obsidian Slot: 3` | **Single Press**: Switches to `Obsidian Slot` -> Right-clicks -> Waits `Delay` -> Switches to `Crystal Slot` -> Right-clicks -> Waits `Delay` -> Left-clicks to detonate.<br>**Hold Key**: Continuously loops *Right-click (place crystal) -> Wait `Delay` -> Left-click (break crystal) -> Wait `Delay`* until released. |
| **Single Anchor** | `SA` | `50 ms` | `Anchor Slot: 5`, `Glowstone Slot: 6`, `Totem/Explode Slot: 8` | Switches to `Anchor Slot` -> Right-clicks to place Respawn Anchor -> Waits `Delay` -> Switches to `Glowstone Slot` -> Right-clicks to charge -> Waits `Delay` -> Switches to `Totem/Explode Slot` -> Right-clicks to detonate safely. |
| **Double Anchor** | `DA` | `75 ms` | `Anchor Slot: 5`, `Glowstone Slot: 6`, `Totem/Explode Slot: 8` | Places and charges the first Respawn Anchor -> Switches to `Anchor Slot`, detonates the first anchor, and immediately places a second anchor in the same spot -> Charges the second anchor with Glowstone -> Switches to `Totem/Explode Slot` and detonates the second anchor. |
| **Anchor Pearl** | `AP` | `50 ms` | `Anchor Slot: 5`, `Glowstone Slot: 6`, `Pearl Slot: 8` | Places Respawn Anchor (`Anchor Slot` + Right-click) -> Waits `Delay` -> Charges with Glowstone (`Glowstone Slot` + Right-click) -> Waits `Delay` -> Switches to `Pearl Slot` -> Right-clicks to detonate anchor -> Waits `50 ms` -> Right-clicks again to throw an Ender Pearl for instant low-ground positioning. |
| **Slow Hit Crystal** | `SHC` | `50 ms` | `Crystal Slot: 2`, `Obsidian Slot: 3` | Places Obsidian (`Obsidian Slot` + Right-click) -> Waits `Delay` -> Switches to `Crystal Slot` -> Right-clicks -> Waits `100 ms` -> Left-clicks.<br>**Hold Key**: Loops *Right-click -> Wait `200 ms` -> Left-click -> Wait `Delay`* to synchronize with opponent hurt-time frames. |
| **Key Pearl** | `KP` | `50 ms` | `Pearl Slot: 2`, `Swap Back Slot: 3` | Switches to `Pearl Slot` -> Right-clicks to throw an Ender Pearl -> Waits `Delay` -> Automatically switches back to `Swap Back Slot`. |
| **Inventory D-Hand** | `IDH` | `50 ms` | `Totem Slot: 8` | **Special Setup**: Bind this macro's `Keybind` to your **Open Inventory key (default `E`)**. When you open your inventory while hovering over a Totem, it automatically presses `Totem Slot` to equip a backup Totem in your main hand. |
| **Offhand Hotbar Totem** | `OHT` | `50 ms` | `Totem Slot: 8`, `Swap Hands Bind: F` | Switches to `Totem Slot` on your hotbar -> Waits `Delay` -> Presses `Swap Hands Bind` (default `F`) to move the Totem into your offhand slot mid-fight. |
| **Fast XP** | `FXP` | `50 ms` | *(No slots required)* | **Hold Key**: Spams Right-click continuously while holding the trigger key with randomized timing jitter (`Delay ± 5 ms`) to mend armor rapidly. |

### Category 2: Mace Macros (4 Macros)

| Macro Name | Badge | Default Delay | Default Slot Bindings | Execution Sequence & Hold Behavior |
| :--- | :--- | :--- | :--- | :--- |
| **Stun Slam** | `SS` | `80 ms` | `Axe slot: 8`, `Mace Slot: 7` | Switches to `Axe slot` -> Left-clicks to disable the opponent's shield -> Waits `Delay` -> Switches to `Mace Slot`.<br>**Hold Key**: Continues Left-clicking with the Mace every `Delay` ms while the key remains held. |
| **Pearl Catch** | `PC` | `50 ms` | `Pearl slot: 4`, `Wind Charge Slot: 2` | Switches to `Pearl slot` -> Right-clicks to throw pearl -> Waits `Delay` -> Switches to `Wind Charge Slot` -> Right-clicks to launch upward with a Wind Charge. |
| **Elytra Swap** | `ES` | `50 ms` | `Elytra/Chestplate Slot: 2` | Switches to `Elytra/Chestplate Slot` -> Waits `Delay` -> Right-clicks to quick-equip your Chestplate or Elytra from the hotbar. |
| **Breach Swap** | `BS` | `50 ms` | `Mace Slot: 5`, `Sword Slot: 1` | **Automatic Left-Click Trigger**: When `Enabled`, every Left-click attack automatically switches to `Mace Slot` -> Waits `Delay` -> Switches back to `Sword Slot` to transfer Mace Breach armor penetration to your swing. |

### Category 3: Sword Macros (1 Macro)

| Macro Name | Badge | Default Delay | Default Slot Bindings | Execution Sequence & Hold Behavior |
| :--- | :--- | :--- | :--- | :--- |
| **Auto Shield Breaker** | `ASB` | `50 ms` | `Axe Slot: 8`, `Sword Slot: 1` | Switches to `Axe Slot` -> Left-clicks to break the opponent's shield -> Waits `Delay` -> Switches back to `Sword Slot`. |

## 5. Troubleshooting

* **Macros do not trigger inside Minecraft**:
  1. Ensure the **Enabled** checkbox on the macro card is checked.
  2. Ensure **Keybind** is set to a valid key (not `None`).
  3. Restart `198Macros.exe` as **Administrator** (`Run as administrator`) so Windows allows low-level hooks and `SendInput` across elevated windows.
* **Hotbar slots do not switch to the expected item**:
  * Verify that the slot keys configured on each card match your in-game Minecraft hotbar keybinds (**Options -> Controls -> Key Binds**), and switch your OS keyboard layout/IME to English (`EN`) while playing so letter keybinds (`F`, `R`, `E`) are not intercepted by an input method editor.
* **Resetting all settings to factory defaults**:
  * Close the application, delete `%USERPROFILE%\.198macros\config.json`, and relaunch the executable to regenerate a fresh configuration file.
