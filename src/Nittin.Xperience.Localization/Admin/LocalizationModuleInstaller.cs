using CMS.ContentEngine;
using CMS.DataEngine;
using CMS.FormEngine;
using CMS.Modules;

namespace Nittin.Xperience.Localization.Admin;

internal class LocalizationModuleInstaller
{
    private readonly IInfoProvider<ResourceInfo> resourceProvider;

    public LocalizationModuleInstaller(IInfoProvider<ResourceInfo> resourceProvider) => this.resourceProvider = resourceProvider;

    public void Install()
    {
        var resource = resourceProvider.Get("Nittin.Xperience.Localization") ?? new ResourceInfo();

        InitializeResource(resource);
        InstallLocalizationKeyInfo(resource);
        InstallLocalizationTranslationInfo(resource);
    }

    public ResourceInfo InitializeResource(ResourceInfo resource)
    {
        resource.ResourceDisplayName = "NITTIN - Xperience Localization";

        resource.ResourceName = "Nittin.Xperience.Localization";
        resource.ResourceDescription = "Localization module data";
        resource.ResourceIsInDevelopment = false;
        if (resource.HasChanged)
        {
            resourceProvider.Set(resource);
        }

        return resource;
    }

    public void InstallLocalizationKeyInfo(ResourceInfo resource)
    {
        var info = DataClassInfoProvider.GetDataClassInfo(LocalizationKeyInfo.OBJECT_TYPE) ?? DataClassInfo.New(LocalizationKeyInfo.OBJECT_TYPE);

        info.ClassName = LocalizationKeyInfo.TYPEINFO.ObjectClassName;
        info.ClassTableName = LocalizationKeyInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
        info.ClassDisplayName = "Localization Key";
        info.ClassType = ClassType.OTHER;
        info.ClassResourceID = resource.ResourceID;

        var formInfo = FormHelper.GetBasicFormDefinition(nameof(LocalizationKeyInfo.LocalizationKeyItemId));

        var formItem = new FormFieldInfo
        {
            Name = nameof(LocalizationKeyInfo.LocalizationKeyItemGuid),
            AllowEmpty = false,
            Visible = true,
            Precision = 0,
            DataType = "guid",
            Enabled = true,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(LocalizationKeyInfo.LocalizationKeyItemName),
            AllowEmpty = false,
            Visible = true,
            Precision = 0,
            Size = 300,
            DataType = "text",
            Enabled = true
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(LocalizationKeyInfo.LocalizationKeyItemDescription),
            AllowEmpty = true,
            Visible = true,
            Precision = 0,
            DataType = "longtext",
            Enabled = true
        };
        formInfo.AddFormItem(formItem);

        SetFormDefinition(info, formInfo);

        if (info.HasChanged)
        {
            DataClassInfoProvider.SetDataClassInfo(info);
        }
    }

    public void InstallLocalizationTranslationInfo(ResourceInfo resource)
    {
        var info = DataClassInfoProvider.GetDataClassInfo(LocalizationTranslationItemInfo.OBJECT_TYPE) ?? DataClassInfo.New(LocalizationTranslationItemInfo.OBJECT_TYPE);

        info.ClassName = LocalizationTranslationItemInfo.TYPEINFO.ObjectClassName;
        info.ClassTableName = LocalizationTranslationItemInfo.TYPEINFO.ObjectClassName.Replace(".", "_");
        info.ClassDisplayName = "Localization Translation";
        info.ClassType = ClassType.OTHER;
        info.ClassResourceID = resource.ResourceID;

        var formInfo = FormHelper.GetBasicFormDefinition(nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemID));

        var formItem = new FormFieldInfo
        {
            Name = nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemGuid),
            AllowEmpty = false,
            Visible = true,
            Precision = 0,
            DataType = "guid",
            Enabled = true,
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemLocalizationKeyItemId),
            AllowEmpty = false,
            Visible = true,
            Precision = 0,
            DataType = "integer",
            ReferenceToObjectType = LocalizationKeyInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Required
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemContentLanguageId),
            AllowEmpty = false,
            Visible = true,
            Precision = 0,
            DataType = "integer",
            ReferenceToObjectType = ContentLanguageInfo.OBJECT_TYPE,
            ReferenceType = ObjectDependencyEnum.Required
        };
        formInfo.AddFormItem(formItem);

        formItem = new FormFieldInfo
        {
            Name = nameof(LocalizationTranslationItemInfo.LocalizationTranslationItemText),
            AllowEmpty = true,
            Visible = true,
            Precision = 0,
            DataType = "longtext",
            Enabled = true
        };
        formInfo.AddFormItem(formItem);

        SetFormDefinition(info, formInfo);

        if (info.HasChanged)
        {
            DataClassInfoProvider.SetDataClassInfo(info);
        }
    }

    /// <summary>
    /// Ensure that the form is upserted with any existing form
    /// </summary>
    /// <param name="info"></param>
    /// <param name="form"></param>
    private static void SetFormDefinition(DataClassInfo info, FormInfo form)
    {
        if (info.ClassID > 0)
        {
            var existingForm = new FormInfo(info.ClassFormDefinition);
            existingForm.CombineWithForm(form, new());
            info.ClassFormDefinition = existingForm.GetXmlDefinition();
        }
        else
        {
            info.ClassFormDefinition = form.GetXmlDefinition();
        }
    }
}
