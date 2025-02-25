namespace WebMain.Settings;

public class MetricSettings
{
    /// <summary>
    /// Путь метрик
    /// </summary>
    public string Patch { get; set; } = "/metrics";

    /// <summary>
    /// Путь проверок доступности
    /// </summary>
    public string HealthPatch { get; set; } = "/health";

    /// <summary>
    /// IP-адрес для REST взаимодействия
    /// </summary>
    public string RestIP { get; set; }

    /// <summary>
    /// Порт для REST взаимодействия
    /// </summary>
    public int RestPort { get; set; }
}
