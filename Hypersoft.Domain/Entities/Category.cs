namespace Hypersoft.Domain.Entities;

public class Category
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public string nome { get; set; } = string.Empty;
    public string descricao { get; set; } = string.Empty;
}
