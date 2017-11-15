using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValues {
    private static string personalKey = "app_id=6a9f2d9b&app_key=56d204570819f80ab478e90e0743bf88";
    private static string secretKey = "ce20add99825303b";

    public static string GetPersonalKey() { return personalKey; }
    public static string GetSecretKey() { return secretKey; }
}
