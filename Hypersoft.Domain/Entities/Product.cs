namespace Hypersoft.Domain.Entities;

public class Product
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
    public decimal preco { get; set; }
    public int quantidade_estoque { get; set; }
    public string categoria_id { get; set; } = string.Empty;

}
