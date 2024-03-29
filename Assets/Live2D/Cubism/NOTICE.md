[English](NOTICE.md) / [日本語](NOTICE.ja.md)

---

# Notices

## [Caution] Operation on the Apple Silicon version of Unity Editor (2023-01-26)

To use Cubism Core for macOS on the Apple Silicon version of the Unity Editor, you need to modify the `Live2DCubismCore.bundle` under `Assets/Live2D/Cubism/Plugins/macOS` from the inspector.
The procedure is as follows:

1. Select `Live2DCubismCore.bundle` and display the inspector.
1. Go to `Platform Settings` > `Editor` and select `Apple Silicon` or `Any CPU`.
1. Restart the Unity Editor.


## [Caution] Support for Windows 11 (2021-12-09)

Regarding Windows 11 compatibility, we have confirmed that the deliverables work on Windows 11.
However, please note that we do not guarantee the operation of builds using Windows 11.
Supported version will be announced with a future release.


### [Caution] About using `.bundle` and `.dylib` on macOS Catalina or later

To use `.bundle` and `.dylib` on macOS Catalina or later, you need to be connected to an online environment to verify your notarization.

For details, please check the official Apple documentation.

* [Apple Official Documentation](https://developer.apple.com/documentation/security/notarizing_your_app_before_distribution)
---

©Live2D
