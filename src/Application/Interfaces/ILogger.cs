namespace Application.Interfaces;
/// <summary>
/// Just my way of doing logging, but not to everyone's liking.
/// The current setup provides us with A LOT of information.
/// It also forces to expand on the function that is doing the calling.
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Logs Info
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    void Info(string messageTemplate, string callingfunction, params object[] objects);
    /// <summary>
    /// Logs Warning
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    void Warn(string messageTemplate, string callingfunction, params object[] objects);
    /// <summary>
    /// Logs Error
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    void Error(string messageTemplate, string callingfunction, params object[] objects);
    /// <summary>
    /// Logs Fatal
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    void Fatal(string messageTemplate, string callingfunction, params object[] objects);
    /// <summary>
    /// Logs Debug
    /// </summary>
    /// <param name="messageTemplate"></param>
    /// <param name="callingfunction"></param>
    /// <param name="objects"></param>
    void Debug(string messageTemplate, string callingfunction, params object[] objects);
}
