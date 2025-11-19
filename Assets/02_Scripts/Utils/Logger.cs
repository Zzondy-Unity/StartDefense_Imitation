using System.Diagnostics;

/// <summary>
/// 개발 및 디버깅을 위한 로깅 유틸리티 클래스입니다.
/// ver_DEV 조건부 컴파일 심볼이 정의된 경우에만 일반 로그와 경고 로그가 출력됩니다.
/// </summary>
public static class Logger
{
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

    /// <summary>
    /// 일반 로그를 출력합니다. (개발 빌드에서만 동작)
    /// </summary>
    /// <param name="message">출력할 메시지</param>
    [Conditional("ver_DEV")]
    public static void Log(string message)
    {
        UnityEngine.Debug.LogFormat("[{0}] {1}", System.DateTime.Now.ToString(DateTimeFormat), message);
    }

    /// <summary>
    /// 경고 로그를 출력합니다. (개발 빌드에서만 동작)
    /// </summary>
    /// <param name="message">출력할 메시지</param>
    [Conditional("ver_DEV")]
    public static void WarningLog(string message)
    {
        UnityEngine.Debug.LogWarningFormat("[{0}] {1}", System.DateTime.Now.ToString(DateTimeFormat), message);
    }

    /// <summary>
    /// 에러 로그를 출력합니다. (모든 빌드에서 동작)
    /// </summary>
    /// <param name="message">출력할 메시지</param>
    public static void ErrorLog(string message)
    {
        UnityEngine.Debug.LogErrorFormat("[{0}] {1}", System.DateTime.Now.ToString(DateTimeFormat), message);
    }
}