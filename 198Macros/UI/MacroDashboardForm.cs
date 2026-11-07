using System.Drawing;
using System.Windows.Forms;
using Macros198.Config;
using Macros198.Input;
using Macros198.Macros;
using Macros198.Models;

namespace Macros198.UI;

/// <summary>
/// Desktop UI form replicating the Direct2D/DirectWrite interface of 198macros.exe
/// (0x1401561a0–0x14015bf10), with the login screen gate (0x1401594f0) permanently removed.
/// </summary>
public sealed class MacroDashboardForm : Form
{
    private readonly ConfigurationManager _configManager;
    private readonly MacroRegistry _macroRegistry;
    private readonly GlobalInputDispatcher _inputDispatcher;

    private MacroCategory _selectedCategory = MacroCategory.Crystal;
    private bool _showingSettingsTab;

    private readonly Panel _sidebarPanel = new();
    private readonly Panel _headerPanel = new();
    private readonly FlowLayoutPanel _cardsPanel = new();

    public MacroDashboardForm(
        ConfigurationManager configManager,
        MacroRegistry macroRegistry,
        GlobalInputDispatcher inputDispatcher)
    {
        _configManager = configManager;
        _macroRegistry = macroRegistry;
        _inputDispatcher = inputDispatcher;

        Text = "198 Macros v1.4.0";
        Size = new Size(980, 680);
        MinimumSize = new Size(880, 560);
        StartPosition = FormStartPosition.CenterScreen;
        DoubleBuffered = true;

        _inputDispatcher.ToggleUiCallback = () =>
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ToggleVisibility));
            }
            else
            {
                ToggleVisibility();
            }
        };

        InitializeLayout();
        RefreshThemeAndContent();
    }

    private void ToggleVisibility()
    {
        Visible = !Visible;
        if (Visible)
        {
            Activate();
        }
    }

    private void InitializeLayout()
    {
        _sidebarPanel.Dock = DockStyle.Left;
        _sidebarPanel.Width = 220;
        _sidebarPanel.Padding = new Padding(16, 20, 16, 20);

        var mainContainer = new Panel { Dock = DockStyle.Fill };

        _headerPanel.Dock = DockStyle.Top;
        _headerPanel.Height = 84;
        _headerPanel.Padding = new Padding(24, 18, 24, 10);

        _cardsPanel.Dock = DockStyle.Fill;
        _cardsPanel.AutoScroll = true;
        _cardsPanel.FlowDirection = FlowDirection.TopDown;
        _cardsPanel.WrapContents = false;
        _cardsPanel.Padding = new Padding(24, 8, 24, 24);

        mainContainer.Controls.Add(_cardsPanel);
        mainContainer.Controls.Add(_headerPanel);

        Controls.Add(mainContainer);
        Controls.Add(_sidebarPanel);
    }

    public void RefreshThemeAndContent()
    {
        ThemePalette theme = _configManager.ActiveTheme;

        BackColor = theme.BackgroundColor;
        _sidebarPanel.BackColor = theme.CardColor;
        _headerPanel.BackColor = theme.BackgroundColor;
        _cardsPanel.BackColor = theme.BackgroundColor;

        RebuildSidebar(theme);
        RebuildHeader(theme);
        RebuildCards(theme);
    }

    private void RebuildSidebar(ThemePalette theme)
    {
        _sidebarPanel.Controls.Clear();
        int y = 20;

        var brandLabel = new Label
        {
            Text = "198 Macros",
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = theme.AccentColor,
            Location = new Point(16, y),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(brandLabel);
        y += 32;

        var statusLabel = new Label
        {
            Text = "v1.4.0 • Connected",
            Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
            ForeColor = theme.MutedTextColor,
            Location = new Point(18, y),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(statusLabel);
        y += 36;

        var macrosHeader = new Label
        {
            Text = "MACROS",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            ForeColor = theme.MutedTextColor,
            Location = new Point(18, y),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(macrosHeader);
        y += 24;

        foreach (MacroCategory cat in Enum.GetValues<MacroCategory>())
        {
            bool active = !_showingSettingsTab && _selectedCategory == cat;
            var btn = CreateSidebarButton(cat.GetShortLabel(), y, active, theme);
            btn.Click += (_, _) =>
            {
                _showingSettingsTab = false;
                _selectedCategory = cat;
                RefreshThemeAndContent();
            };
            _sidebarPanel.Controls.Add(btn);
            y += 44;
        }

        y += 16;
        var otherHeader = new Label
        {
            Text = "OTHER",
            Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
            ForeColor = theme.MutedTextColor,
            Location = new Point(18, y),
            AutoSize = true
        };
        _sidebarPanel.Controls.Add(otherHeader);
        y += 24;

        var settingsBtn = CreateSidebarButton("Themes & Settings", y, _showingSettingsTab, theme);
        settingsBtn.Click += (_, _) =>
        {
            _showingSettingsTab = true;
            RefreshThemeAndContent();
        };
        _sidebarPanel.Controls.Add(settingsBtn);
        y += 52;

        var saveBtn = CreateSidebarButton("Save Config", y, false, theme);
        saveBtn.Click += (_, _) => _configManager.SaveToFile();
        _sidebarPanel.Controls.Add(saveBtn);
    }

    private static Button CreateSidebarButton(string text, int y, bool active, ThemePalette theme)
    {
        return new Button
        {
            Text = text,
            Location = new Point(16, y),
            Size = new Size(188, 38),
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10f, active ? FontStyle.Bold : FontStyle.Regular),
            BackColor = active ? theme.AccentColor : theme.BackgroundColor,
            ForeColor = active ? Color.White : theme.TextColor,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
            FlatAppearance = { BorderSize = 0 }
        };
    }

    private void RebuildHeader(ThemePalette theme)
    {
        _headerPanel.Controls.Clear();

        string titleText = _showingSettingsTab ? "Themes & Settings" : _selectedCategory.GetHeaderTitle();
        string subText = _showingSettingsTab
            ? "Customize the appearance of your interface"
            : _selectedCategory.GetSubtitle();

        var titleLabel = new Label
        {
            Text = titleText,
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            ForeColor = theme.TextColor,
            Location = new Point(24, 16),
            AutoSize = true
        };

        var subLabel = new Label
        {
            Text = subText,
            Font = new Font("Segoe UI", 10f, FontStyle.Regular),
            ForeColor = theme.MutedTextColor,
            Location = new Point(26, 48),
            AutoSize = true
        };

        _headerPanel.Controls.Add(titleLabel);
        _headerPanel.Controls.Add(subLabel);
    }

    private void RebuildCards(ThemePalette theme)
    {
        _cardsPanel.SuspendLayout();
        _cardsPanel.Controls.Clear();

        if (_showingSettingsTab)
        {
            _cardsPanel.Controls.Add(BuildSettingsCard(theme));
            _cardsPanel.ResumeLayout();
            return;
        }

        foreach (var card in _configManager.GetAllMacroConfigs())
        {
            if (card.Category == _selectedCategory)
            {
                _cardsPanel.Controls.Add(BuildMacroCardPanel(card, theme));
            }
        }

        _cardsPanel.ResumeLayout();
    }

    private Panel BuildMacroCardPanel(MacroConfiguration card, ThemePalette theme)
    {
        var panel = new Panel
        {
            Size = new Size(680, 118),
            BackColor = theme.CardColor,
            Margin = new Padding(0, 0, 0, 12),
            Padding = new Padding(16)
        };

        var badgeLabel = new Label
        {
            Text = card.BadgeCode,
            Size = new Size(42, 42),
            Location = new Point(16, 16),
            BackColor = theme.AccentColor,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10.5f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleCenter
        };
        panel.Controls.Add(badgeLabel);

        var nameLabel = new Label
        {
            Text = card.Name,
            Location = new Point(70, 14),
            AutoSize = true,
            Font = new Font("Segoe UI", 11.5f, FontStyle.Bold),
            ForeColor = theme.TextColor
        };
        panel.Controls.Add(nameLabel);

        var descLabel = new Label
        {
            Text = card.Description.Replace("
", " "),
            Location = new Point(70, 38),
            AutoSize = true,
            Font = new Font("Segoe UI", 9f, FontStyle.Regular),
            ForeColor = theme.MutedTextColor
        };
        panel.Controls.Add(descLabel);

        var enabledCheck = new CheckBox
        {
            Text = "Enabled",
            Checked = card.Enabled,
            Location = new Point(575, 18),
            AutoSize = true,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            ForeColor = theme.TextColor
        };
        enabledCheck.CheckedChanged += (_, _) =>
        {
            card.Enabled = enabledCheck.Checked;
            _configManager.SaveToFile();
        };
        panel.Controls.Add(enabledCheck);

        int x = 16;
        int rowY = 74;

        var kbTitle = new Label
        {
            Text = "Keybind:",
            Location = new Point(x, rowY + 4),
            AutoSize = true,
            ForeColor = theme.TextColor,
            Font = new Font("Segoe UI", 9f)
        };
        panel.Controls.Add(kbTitle);
        x += 58;

        var kbBtn = new Button
        {
            Text = card.Keybind,
            Location = new Point(x, rowY),
            Size = new Size(88, 28),
            FlatStyle = FlatStyle.Flat,
            BackColor = theme.BackgroundColor,
            ForeColor = theme.AccentColor,
            Font = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        kbBtn.Click += (_, _) =>
        {
            kbBtn.Text = "Press key...";
            _inputDispatcher.KeybindCaptureCallback = vk =>
            {
                string keyName = vk == KeybindMapper.VK_ESCAPE ? "None" : KeybindMapper.ToKeyName(vk);
                card.Keybind = keyName;
                _configManager.SaveToFile();
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => kbBtn.Text = card.Keybind));
                }
                else
                {
                    kbBtn.Text = card.Keybind;
                }
            };
        };
        panel.Controls.Add(kbBtn);
        x += 102;

        var delayTitle = new Label
        {
            Text = "Delay (ms):",
            Location = new Point(x, rowY + 4),
            AutoSize = true,
            ForeColor = theme.TextColor,
            Font = new Font("Segoe UI", 9f)
        };
        panel.Controls.Add(delayTitle);
        x += 72;

        var delaySpinner = new NumericUpDown
        {
            Minimum = 0,
            Maximum = 1000,
            Increment = 5,
            Value = Math.Clamp(card.DelayMs, 0, 1000),
            Location = new Point(x, rowY + 2),
            Size = new Size(64, 26),
            BackColor = theme.BackgroundColor,
            ForeColor = theme.TextColor
        };
        delaySpinner.ValueChanged += (_, _) =>
        {
            card.DelayMs = (int)delaySpinner.Value;
            _configManager.SaveToFile();
        };
        panel.Controls.Add(delaySpinner);
        x += 78;

        foreach (var slotKv in card.Slots.ToList())
        {
            string slotKey = slotKv.Key;
            var slotLbl = new Label
            {
                Text = $"{slotKey}:",
                Location = new Point(x, rowY + 4),
                AutoSize = true,
                ForeColor = theme.TextColor,
                Font = new Font("Segoe UI", 9f)
            };
            panel.Controls.Add(slotLbl);
            x += slotLbl.PreferredWidth + 4;

            var slotBox = new TextBox
            {
                Text = slotKv.Value,
                Location = new Point(x, rowY + 2),
                Size = new Size(36, 26),
                BackColor = theme.BackgroundColor,
                ForeColor = theme.TextColor
            };
            slotBox.TextChanged += (_, _) =>
            {
                card.SetSlotValue(slotKey, slotBox.Text);
                _configManager.SaveToFile();
            };
            panel.Controls.Add(slotBox);
            x += 48;
        }

        return panel;
    }

    private Panel BuildSettingsCard(ThemePalette theme)
    {
        var panel = new Panel
        {
            Size = new Size(680, 140),
            BackColor = theme.CardColor,
            Padding = new Padding(16)
        };

        var themeLbl = new Label
        {
            Text = "Theme Palette (15 Built-in Direct2D Themes):",
            Location = new Point(16, 20),
            AutoSize = true,
            ForeColor = theme.TextColor,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold)
        };
        panel.Controls.Add(themeLbl);

        var themeCombo = new ComboBox
        {
            Location = new Point(340, 16),
            Size = new Size(300, 28),
            DropDownStyle = ComboBoxStyle.DropDownList,
            BackColor = theme.BackgroundColor,
            ForeColor = theme.TextColor
        };
        foreach (var p in ThemePalette.All)
        {
            themeCombo.Items.Add(p);
        }
        themeCombo.SelectedItem = _configManager.ActiveTheme;
        themeCombo.SelectedIndexChanged += (_, _) =>
        {
            if (themeCombo.SelectedItem is ThemePalette selected)
            {
                _configManager.ActiveTheme = selected;
                _configManager.SaveToFile();
                RefreshThemeAndContent();
            }
        };
        panel.Controls.Add(themeCombo);

        var toggleLbl = new Label
        {
            Text = "Toggle UI Keybind:",
            Location = new Point(16, 70),
            AutoSize = true,
            ForeColor = theme.TextColor,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold)
        };
        panel.Controls.Add(toggleLbl);

        var toggleBtn = new Button
        {
            Text = _configManager.ToggleUiKeybind,
            Location = new Point(340, 66),
            Size = new Size(120, 30),
            FlatStyle = FlatStyle.Flat,
            BackColor = theme.BackgroundColor,
            ForeColor = theme.AccentColor,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold)
        };
        toggleBtn.Click += (_, _) =>
        {
            toggleBtn.Text = "Press key...";
            _inputDispatcher.KeybindCaptureCallback = vk =>
            {
                _configManager.ToggleUiKeybind =
                    vk == KeybindMapper.VK_ESCAPE ? "None" : KeybindMapper.ToKeyName(vk);
                _configManager.SaveToFile();
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => toggleBtn.Text = _configManager.ToggleUiKeybind));
                }
                else
                {
                    toggleBtn.Text = _configManager.ToggleUiKeybind;
                }
            };
        };
        panel.Controls.Add(toggleBtn);

        return panel;
    }
}
