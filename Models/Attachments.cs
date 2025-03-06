namespace ASP.NETCore;

using System.ComponentModel.DataAnnotations;

public class Attachment
{
    [Key]
    public int Serial { get; private set; }

    [Required]
    public int PostSerial { get; private set; }
    public string FilePath { get; private set; }

    public Attachment() { }

    public Attachment(int postSerial, string filePath)
        : this()
    {
        PostSerial = postSerial;
        FilePath = filePath;
    }
}
