namespace Client.Wasm.DTOs;

using Microsoft.AspNetCore.Components.Forms;

public class DocumentClassifyFormDto
{
    public string? Text { get; set; }
    public IBrowserFile? File { get; set; }
}
