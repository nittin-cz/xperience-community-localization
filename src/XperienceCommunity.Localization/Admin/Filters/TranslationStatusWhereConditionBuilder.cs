using CMS.DataEngine;

using Kentico.Xperience.Admin.Base.Filters;

namespace XperienceCommunity.Localization.Admin.Filters;

public sealed class TranslationStatusWhereConditionBuilder(IInfoProvider<LocalizationTranslationItemInfo> localizationTranslationItemInfoProvider) : IWhereConditionBuilder
{
    public Task<IWhereCondition> Build(string columnName, object value)
    {
        var whereCondition = new WhereCondition();

        if (value is not string selectedValue || string.IsNullOrEmpty(selectedValue))
        {
            return Task.FromResult<IWhereCondition>(whereCondition);
        }

        var parts = selectedValue.Split(':', 2);

        if (parts.Length != 2 || !int.TryParse(parts[1], out int languageId))
        {
            return Task.FromResult<IWhereCondition>(whereCondition);
        }

        var keyIdsWithTranslation = localizationTranslationItemInfoProvider.Get()
            .WhereEquals(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId), languageId)
            .WhereNotEmpty(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemText))
            .Column(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId));

        if (parts[0] == "missing")
        {
            whereCondition.WhereNotIn(columnName, keyIdsWithTranslation);
        }
        else
        {
            whereCondition.WhereIn(columnName, keyIdsWithTranslation);
        }

        return Task.FromResult<IWhereCondition>(whereCondition);
    }
}
