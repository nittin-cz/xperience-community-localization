namespace XperienceCommunity.Localization;

public enum LocalizationModificationResultState
{
    Success,
    Failure
}

public class LocalizationModificationResult
{
    public LocalizationModificationResultState LocalizationModificationResultState { get; set; }
    public string? Message { get; set; }
    public LocalizationModificationResult(LocalizationModificationResultState resultState, string? message = null)
    {
        LocalizationModificationResultState = resultState;
        Message = message;
    }
}
