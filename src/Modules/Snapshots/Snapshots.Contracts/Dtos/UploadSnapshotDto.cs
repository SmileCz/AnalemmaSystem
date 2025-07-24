namespace Snapshots.Contracts;

public class UploadSnapshotDto
{
    public string FileName { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public GpsData Gps { get; set; }
    public string Exposure { get; set; }
    public int Iso { get; set; }
    public double FocalLength { get; set; }
    public int Orientation { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string ImageBase64 { get; set; }
    public DeviceData Device { get; set; }
    public SunPosition SunPosition { get; set; }
    
}