
# 🧪 ToggleButtonAutomation

### Automated UI Testing for Windows Desktop Apps

This repository implements a lightweight test automation framework to validate the behavior of the provided `Toggle button.exe` desktop applications.

---

## 📋 Assignment Overview

You were given three versions of the same app (`Toggle button.exe`), each behaving slightly differently:

| Folder | Expected Result | Description |
|:--------|:----------------|:-------------|
| **ToggleButton** | ✅ Pass | Application launches and toggles correctly |
| **ToggleButton_NoStart** | ❌ Fail | Application fails to launch |
| **ToggleButton_WrongBehavior** | ❌ Fail | Application launches but toggle logic is incorrect |

The goal is to:
1. Create a **reusable test framework** for future desktop app testing.
2. Implement **automated tests** validating correct toggle behavior.
3. Produce **clear, informative logs** for both passing and failing cases.

---

## 🧠 Framework Design

### Key Concepts
- **UI automation:** Interaction through [FlaUI](https://github.com/FlaUI/FlaUI) (UIA3).
- **Image validation:** Screenshots of the toggle area (ROI) are compared using **SSIM** to measure similarity.
- **No baseline images:** Comparison is performed dynamically between captured states within the same test run.
- **SmartScreen & Firewall dialogs:** Automatically bypassed to prevent blocking the automation flow.
- **Serilog reporting:** Logs key steps, SSIM values, and errors for every executable.

---

## 🧩 Project Structure

```
ToggleButtonAutomation/
│
├── .github/
│   └── workflows/
│       └── release-pipeline.yaml         # GitHub Actions CI/CD
│
├── Framework/
│   ├── Extensions/
│   │   └── WindowExtensions.cs          # Helper extensions for window handling
│   ├── Utils/
│   │   ├── FirewallRules.cs             # Adds firewall allow rules for app
│   │   ├── ImageUtils.cs                # SSIM calculation and diff creation
│   │   ├── ScreenUtils.cs               # ROI and screenshot handling
│   │   └── SmartScreenBypass.cs         # Handles SmartScreen “Run anyway” dialog
│   ├── AppLauncher.cs                   # Launch and monitor desktop process
│   ├── UiSession.cs                     # Attach to main window, manage session
│   └── Framework.csproj
│
├── ToggleButtonTests/
│   ├── Pages/
│   │   └── MainWindow.cs                # Defines ROI and UI actions
│   ├── Utils/
│   │   └── TestDataUtil.cs              # Provides paths to test executables
│   ├── appsettings.json                 # Configuration: timeouts, binaries
│   ├── BaseTest.cs                      # Common setup, logging, teardown
│   ├── ToggleTests.cs                   # Main visual toggle verification test
│   └── ToggleButtonTests.csproj
│
├── ToggleButtonAutomation.sln
│
├── ToggleButton/                        # Expected to pass
├── ToggleButton_NoStart/                # Fails to start
└── ToggleButton_WrongBehavior/          # Fails toggle validation
```

---

## ⚙️ Test Logic

The test verifies the **visual change** of the toggle button when clicked:

1. Launch the target `.exe`.
2. Capture a screenshot of the **toggle region** (ROI).
3. Perform the first click → capture the new ROI.
4. Perform the second click → capture the ROI again.
5. Compare screenshots using **SSIM (Structural Similarity Index)**:
   - SSIM < 0.98 after 1st click → toggle **did not change** visually.
   - SSIM ≥ 0.98 after 2nd click → toggle **did not return** to initial state.
6. Log all SSIM values and screenshots for review.

> No baseline files are used — all comparisons happen dynamically during test execution.

---

## 🧾 Output

Each run generates a timestamped folder under `./reports/`:

```
reports/run-YYYYMMDD-HHMMSS/<AppName>/
  ├── 00_state0.png     # Initial
  ├── 01_state1.png     # After 1st click
  ├── 02_state2.png     # After 2nd click
  ├── diff_*.png        # Visual diffs
  └── report.txt        # Detailed log
```

Logs include:
- App launch details
- ROI capture coordinates
- SSIM values
- Test outcome and any failure messages

---

## 🚀 SmartScreen & Firewall Handling

To ensure stable automated runs:

- **SmartScreenBypass.cs** clicks “Run Anyway” in “Windows protected your PC”.
- **FirewallRules.cs** pre-creates inbound/outbound rules via `netsh` to avoid popups.
- If the firewall dialog still appears, **UI automation clicks “Allow”** automatically.

---

## 🧰 Requirements

- **Windows 10/11 x64**
- **.NET 8 SDK** (Desktop)
- Dependencies:
  ```
  FlaUI.UIA3
  Magick.NET-Q16-AnyCPU
  Serilog
  Serilog.Sinks.File
  Serilog.Sinks.Console
  ```

---

## ▶️ How to Run Locally

```powershell
# 1. Restore and build
dotnet restore .\ToggleButtonAutomation\ToggleButtonAutomation.sln
dotnet build   .\ToggleButtonAutomation\ToggleButtonAutomation.sln -c Release

# 2. Run all tests
dotnet test .\ToggleButtonAutomation\ToggleButtonTests\ToggleButtonTests.csproj -c Release
```

---

## 🤖 Continuous Integration

GitHub Actions workflow: `.github/workflows/release-pipeline.yaml`

### Jobs
| Job | Runner | Purpose |
|------|--------|----------|
| **Build** | `windows-latest` | Restores and builds the solution |
| **ui_tests** | `self-hosted, Windows, X64` | Runs interactive UI tests, uploads reports |

> ⚠️ Note: Hosted GitHub runners cannot run UI automation.  
> A **self-hosted Windows runner** with a visible desktop session is required.

---

## 🧠 Evaluation Coverage

✅ **Correctness** – verifies real visual change, not hard-coded references.  
✅ **Code Quality** – modular structure with clear separation of concerns.  
✅ **Error Reporting** – explicit, contextual failure messages.  
✅ **Documentation** – detailed environment setup and execution guide.  

---

## 🧱 Known Limitations

- Window size/position must remain stable during test run.  
- SmartScreen and Firewall dialogs may appear based on Windows settings.  

---

**Author:** Yauheni Chyruk  
**Tech Stack:** .NET 8, FlaUI, Magick.NET, Serilog  
**License:** Internal Assessment Project
