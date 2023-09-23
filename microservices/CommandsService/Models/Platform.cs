namespace CommandsService.Models;

public class Platform
{
    public required int Id { get; set; }
    public required int ExternalId { get; set; }
    public required string Name { get; set; }

    public required ICollection<Command> Commands { get; set; } = new List<Command>();
}