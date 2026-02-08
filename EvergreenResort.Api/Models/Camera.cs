namespace EvergreenResort.Api.Models;

public class Camera 
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Descrizione { get; set; } = string.Empty;
    public decimal Prezzo { get; set; }
    public string ImmagineUrl { get; set; } = string.Empty;
    public string Stato { get; set; } = "Libera"; // Libera, Occupata, In Pulizia, Manutenzione
    public string Tipologia { get; set; } = "Standard"; // Suite, Deluxe, Standard
}