using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using A5Soft.CARMA.Domain.Metadata;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace A5Soft.A5App.WebApp.Components
{
    /// <summary>
    /// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.
    /// </summary>
    public class InputLabel<T> : ComponentBase
    {
        private EditContext _previousEditContext;
        private Expression<Func<T>> _previousFieldAccessor;
        private FieldIdentifier _fieldIdentifier;

        [Inject]
        public IMetadataProvider MetadataProvider { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to the created <c>div</c> element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)] 
        public IReadOnlyDictionary<string, object> AdditionalAttributes { get; set; }

        [CascadingParameter] 
        EditContext CurrentEditContext { get; set; }

        /// <summary>
        /// Specifies the field for which validation messages should be displayed.
        /// </summary>
        [Parameter] 
        public Expression<Func<T>> For { get; set; }


        /// <summary>`
        /// Constructs an instance of <see cref="InputLabel{T}"/>.
        /// </summary>
        public InputLabel() { }


        /// <inheritdoc />
        protected override void OnParametersSet()
        {
            if (CurrentEditContext == null)
            {
                throw new InvalidOperationException($"{GetType()} requires a cascading parameter " +
                    $"of type {nameof(EditContext)}. For example, you can use {GetType()} inside " +
                    $"an {nameof(EditForm)}.");
            }

            if (For == null) // Not possible except if you manually specify T
            {
                throw new InvalidOperationException($"{GetType()} requires a value for the " +
                    $"{nameof(For)} parameter.");
            }
            else if (For != _previousFieldAccessor)
            {
                _fieldIdentifier = FieldIdentifier.Create(For);
                _previousFieldAccessor = For;
            }

            if (CurrentEditContext != _previousEditContext)
            {
                _previousEditContext = CurrentEditContext;
            }
        }

        /// <inheritdoc />
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var metadata = MetadataProvider.GetEntityMetadata(_fieldIdentifier.Model.GetType());
            var content = _fieldIdentifier.FieldName;
            if (metadata.Properties.ContainsKey(_fieldIdentifier.FieldName))
                content = metadata.Properties[_fieldIdentifier.FieldName].GetDisplayName();

            builder.OpenElement(0, "label");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.AddContent(2, content);
            builder.CloseElement();
        }
    }
}
