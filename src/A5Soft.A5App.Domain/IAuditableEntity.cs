using A5Soft.CARMA.Domain;

namespace A5Soft.A5App.Domain
{
    /// <summary>
    /// Common interface for entities that are auditable, i.e. include identity data as well as
    /// data on when it was created and last updated including identifiers of the respective users.
    /// </summary>
    public interface IAuditableEntity : IDomainEntity, IAuditable
    {
    }
}
