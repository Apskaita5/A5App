using A5Soft.CARMA.Domain;
using System;

namespace A5Soft.A5App.Domain.Security.Lookups
{
    /// <summary>
    /// a lookup for <see cref="IUser"/> reference
    /// </summary>
    [Serializable]
    public class UserLookup : LookupBase
    {
        protected bool _isSuspended;
        protected bool _isDisabled;
        private string _occHash;
        [NonSerialized]
        protected DateTime _updatedAt;


        protected UserLookup() { }

                      
        /// <inheritdoc cref="IUser.IsSuspended"/>
        public bool IsSuspended => _isSuspended;

        /// <inheritdoc cref="IUser.IsDisabled"/>
        public bool IsDisabled => _isDisabled;

        /// <inheritdoc cref="IAuditable.OccHash"/>
        public string OccHash => _occHash;


        protected void SetOccHash()
        {
            _occHash = _updatedAt.CreateOccHash<User>();
        }

    }
}
