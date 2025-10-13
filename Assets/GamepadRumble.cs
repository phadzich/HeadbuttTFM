using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadRumble : MonoBehaviour
{
    public static void Vibrate(float low, float high, float duration)
    {
        if (Gamepad.current == null) return;
        if (SettingsManager.instance.vibration == 0) return;

        Gamepad.current.SetMotorSpeeds(low, high);
        Instance?.CancelInvoke(nameof(Stop));
        Instance?.Invoke(nameof(Stop), duration);
    }

    static GamepadRumble Instance;
    void Awake() => Instance = this;
    void Stop() => Gamepad.current?.SetMotorSpeeds(0, 0);


    public static void VibrateMine() => Vibrate(0.3f, 0.3f, 0.15f);
    public static void VibrateHeadbutt() => Vibrate(0.3f, 0.9f, 0.4f);
    public static void VibrateHit() => Vibrate(0.6f, 0.8f, 0.25f);
    public static void VibrateDead() => Vibrate(0.7f, 0.9f, 0.8f);
}