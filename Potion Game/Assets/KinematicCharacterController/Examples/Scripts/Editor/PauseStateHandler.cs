using KinematicCharacterController;
using UnityEditor;
using UnityEngine;

public class PauseStateHandler
{
    [RuntimeInitializeOnLoadMethod()]
    public static void Init()
    {
        EditorApplication.pauseStateChanged += HandlePauseStateChange;
    }

    private static void HandlePauseStateChange(PauseState state)
    {
        foreach (KinematicCharacterMotor motor in KinematicCharacterSystem.CharacterMotors)
        {
            motor.SetPositionAndRotation(motor.Transform.position, motor.Transform.rotation, true);
        }
    }
}
