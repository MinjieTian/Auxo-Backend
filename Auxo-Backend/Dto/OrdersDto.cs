namespace Auxo_Backend.Dto;

public class OrderDto
{
    public required List<SelectedPart> SelectedParts {get; set;}
}

public class SelectedPart {
    public required int Id {get; set;}
    public required int Quantity { get; set;}
}