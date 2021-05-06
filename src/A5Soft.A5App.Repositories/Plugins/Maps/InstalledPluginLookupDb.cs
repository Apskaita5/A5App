using A5Soft.A5App.Application.UseCases.Plugins;
using A5Soft.DAL.Core.MicroOrm;

namespace A5Soft.A5App.Repositories.Plugins.Maps
{
    internal class InstalledPluginLookupDb : InstalledPluginLookup
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static readonly IdentityMapParentInt32Autoincrement<InstalledPluginLookup> _identityMap =
            new IdentityMapParentInt32Autoincrement<InstalledPluginLookup>("extensions", 
            "id", nameof(EntryId), () => new InstalledPluginLookupDb(),
            (c) => ((InstalledPluginLookupDb)c).EntryId,
            (c, v) => ((InstalledPluginLookupDb)c).EntryId = v);
        private static readonly FieldMapGuid<InstalledPluginLookup> _extensionGuidMap =
            new FieldMapGuid<InstalledPluginLookup>("extension_guid", nameof(Id),
            (c, v) => ((InstalledPluginLookupDb)c)._id = v);
        private static readonly FieldMapString<InstalledPluginLookup> _extensionNameMap =
            new FieldMapString<InstalledPluginLookup>("extension_name", nameof(Name),
            (c, v) => ((InstalledPluginLookupDb)c)._name = v);
        private static readonly FieldMapString<InstalledPluginLookup> _extensionVersionMap =
            new FieldMapString<InstalledPluginLookup>("extension_version", nameof(Version),
            (c, v) => ((InstalledPluginLookupDb)c)._version = v);
#pragma warning restore IDE0052 // Remove unread private members

        private int? EntryId { get; set; }

    }
}
