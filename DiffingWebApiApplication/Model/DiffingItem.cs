
/// <summary>
/// Data model.
/// </summary>
public class DiffingItem
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="id">Id of diffing item.</param>
    /// <param name="leftValue">Base64 encoded value of the left operand.</param>
    /// <param name="rightValue">Base64 encoded value of the right operand.</param>
    public DiffingItem(int id, string? leftValue, string? rightValue)
    {
        Id = id;
        LeftValue = leftValue;
        RightValue = rightValue;
    }

    /// <summary>
    /// Id of diffing item.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Base64 encoded value of the left operand
    /// </summary>
    public string? LeftValue { get; set; }

    /// <summary>
    /// Base64 encoded value of the right operand
    /// </summary>
    public string? RightValue { get; set; }
}
