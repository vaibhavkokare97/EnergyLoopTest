using UnityEngine;

public static class SnapToNearestAngle
{
    public static void SnapRotation(Transform t, Vector3 axis, float snapAngle)
    {
        // Get the current rotation of the GameObject
        Vector3 _currentRotation = t.eulerAngles;

        // Snap z axis to the nearest 60 degrees
        if (axis == new Vector3(0, 1, 0))
        {
            _currentRotation.y = SnapValue(t.eulerAngles.y, snapAngle);
        }
        else if (axis == new Vector3(1, 0, 0))
        {
            _currentRotation.x = SnapValue(t.eulerAngles.x, snapAngle);
        }
        else if (axis == new Vector3(0, 0, 1))
        {
            _currentRotation.z = SnapValue(t.eulerAngles.z, snapAngle);
        }
        else
        {
            return;
        }


        // Apply the snapped rotation back to the GameObject
        t.eulerAngles = _currentRotation;
    }

    private static float SnapValue(float value, float snapAngle)
    {
        return Mathf.Round(value / snapAngle) * snapAngle;
    }
}