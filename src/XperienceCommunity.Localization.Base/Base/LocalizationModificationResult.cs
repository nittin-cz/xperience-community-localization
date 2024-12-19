namespace XperienceCommunity.Localization.Base;

public enum LocalizationModificationResultState
{
    Success,
    Failure
}

public class LocalizationModificationResult(LocalizationModificationResultState resultState, string? message = null)
{
    public LocalizationModificationResultState LocalizationModificationResultState { get; set; } = resultState;
    public string? Message { get; set; } = message;
}
