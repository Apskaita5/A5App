using System;
using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Application.UseCases.Plugins
{
    /// <summary>
    /// Describes an application document/operation type that extends built in TEnum type.
    /// </summary>
    public class ExtendedType<TEnum> where TEnum: struct
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id">an id of the extended type</param>
        /// <param name="name">a name of the extended type (only used as a metadata in an app tenant database)</param>
        /// <param name="fallbackValue">a built in document/operation type to fallback to if the plugin is uninstalled</param>
        public ExtendedType(Guid id, string name, TEnum fallbackValue)
        {
            if (!typeof(TEnum).IsEnum) throw new InvalidOperationException(
                $"Extended type should extend an enum while type {typeof(TEnum).FullName} is not.");

            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            if (name.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(name));

            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            FallbackValue = fallbackValue;
        }

        /// <summary>
        /// an id of the extended type
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// a name of the extended type (only used as a metadata in an app tenant database)
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// a built in document/operation type to fallback to if the plugin is uninstalled
        /// </summary>
        public TEnum FallbackValue { get; }

    }
}
