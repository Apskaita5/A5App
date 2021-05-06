using System;
using System.Globalization;
using System.Linq;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Plugins
{
    /// <summary>
    /// a container for basic info about an app extension
    /// </summary>
    [Serializable]
    public class PluginInfo
    {
        private readonly int _intVersion = int.MinValue;
        private readonly DateTime _dateVersion = DateTime.MinValue;
        private readonly int[] _conventionalVersion = null;


        /// <summary>
        /// creates a new instance of an extension info
        /// </summary>
        /// <param name="id">an id of the extension</param>
        /// <param name="name">a name of the extension</param>
        /// <param name="version">a version of the extension</param>
        public PluginInfo(Guid id, string name, string version)
        {
            if (name.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(name));
            if (version.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(version));
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));

            Id = id;
            Name = name.Trim();
            Version = version.Trim();

            if (DateTime.TryParseExact(Version, "yyyyMMdd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime dateVersion))
            {
                _dateVersion = dateVersion.Date;
                return;
            }
            else if (DateTime.TryParseExact(Version, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out DateTime altDateVersion))
            {
                _dateVersion = altDateVersion.Date;
                return;
            }
            else if (!Version.Contains(".") && int.TryParse(Version, out int intVersion))
            {
                _intVersion = intVersion;
                return;
            }
            else if (Version.Contains("."))
            {
                try
                {
                    _conventionalVersion = Version.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(v => int.Parse(v)).ToArray();
                    return;
                }
                catch (Exception) { }
            }

            throw new FormatException(
                $"Version identifier '{Version}' format is invalid, allowed formats: DateTime (yyyy-MM-dd or yyyyMMdd), int or int[].)");
        }


        /// <summary>
        /// an id of the extension
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// a name of the extension
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// a version of the extension
        /// </summary>
        public string Version { get; }


        /// <summary>
        /// Compares plugin (app extensions) versions similar to IComparable.CompareTo method. 
        /// </summary>
        /// <param name="versionToCompare">a plugin (app extension) version to compare to</param>
        /// <returns>Returns 1, if the version of the plugin is newer than of the compared one.
        /// Returns -1, if the version of the plugin is older than of the compared one.
        /// Returns 0, if the versions match.</returns>
        /// <exception cref="ArgumentNullException">if <see langword="null"/> extension</exception>
        /// <exception cref="FormatException">invalid format for the specified version</exception>
        /// <exception cref="InvalidOperationException">versions are defined in different formats</exception>
        public int CompareVersionTo(string versionToCompare)
        {
            return CompareVersionTo(new PluginInfo(Id, Name, versionToCompare));
        }

        /// <summary>
        /// Compares plugin (app extensions) versions similar to IComparable.CompareTo method. 
        /// </summary>
        /// <param name="plugin">a plugin (app extension) to compare to</param>
        /// <returns>Returns 1, if the version of the plugin is newer than of the compared one.
        /// Returns -1, if the version of the plugin is older than of the compared one.
        /// Returns 0, if the versions match.</returns>
        /// <exception cref="ArgumentNullException">if <see langword="null"/> extension</exception>
        /// <exception cref="InvalidOperationException">cannot compare versions of different plugins (app extensions)</exception>
        /// <exception cref="InvalidOperationException">versions are defined in different formats</exception>
        public int CompareVersionTo(PluginInfo plugin)
        {
            if (plugin.IsNull()) throw new ArgumentNullException(nameof(plugin));
            if (plugin.Id != Id) throw new InvalidOperationException(
                "Cannot compare versions of different plugins (extensions).");

            if (null != plugin._conventionalVersion && null != _conventionalVersion)
            {
                for (int i = 0; i < Math.Min(plugin._conventionalVersion.Length, _conventionalVersion.Length); i++)
                {
                    if (plugin._conventionalVersion[i] > _conventionalVersion[i]) return -1;
                    if (plugin._conventionalVersion[i] < _conventionalVersion[i]) return 1;
                }

                return 0;
            }
            else if (plugin._dateVersion != DateTime.MinValue && _dateVersion != DateTime.MinValue)
            {
                if (plugin._dateVersion > _dateVersion) return -1;
                else if (plugin._dateVersion < _dateVersion) return 1;
                return 0;
            }
            else if (plugin._intVersion != int.MinValue && _intVersion != int.MinValue)
            {
                if (plugin._intVersion > _intVersion) return -1;
                else if (plugin._intVersion < _intVersion) return 1;
                return 0;
            }

            throw new InvalidOperationException(
                $"Cannot compare versions defined in different formats: {Version} vs {plugin.Version}.");
        }
                          
    }
}
