using System;
using A5Soft.A5App.Application.Properties;
using A5Soft.A5App.Domain.Core;
using A5Soft.CARMA.Domain.Metadata;
using A5Soft.CARMA.Domain.Rules;

namespace A5Soft.A5App.Application
{
    [Serializable]
    public class NotFoundException : BusinessException
    {
        /// <inheritdoc />
        public NotFoundException(string entityTypeName, string entityId) 
            : base(string.Format(Resources.NotFoundException_Message, entityTypeName, entityId)) { }
                  
        /// <inheritdoc />
        public NotFoundException(Type entityType, string entityId,
            IMetadataProvider metadataProvider)
            : base(string.Format(Resources.NotFoundException_Message,
                metadataProvider.GetEntityMetadata(entityType).GetDisplayNameForOld(),
                entityId)) { }

    }
}
