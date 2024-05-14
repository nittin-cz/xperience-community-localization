namespace XperienceCommunity.Localization.Admin;

internal enum LocalizationModificationResultState
{
    Success,
    Failure
}

internal class LocalizationModificationResult
{
    public LocalizationModificationResultState LocalizationModificationResultState { get; set; }
    public string? Message { get; set; }
    public LocalizationModificationResult(LocalizationModificationResultState resultState, string? message = null)
    {
        LocalizationModificationResultState = resultState;
        Message = message;
    }
}
