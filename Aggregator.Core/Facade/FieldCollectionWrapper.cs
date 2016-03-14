﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

using Aggregator.Core.Context;
using Aggregator.Core.Extensions;
using Aggregator.Core.Interfaces;
using Aggregator.Core.Monitoring;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace Aggregator.Core.Facade
{
    public class FieldCollectionWrapper : IFieldCollection
    {
        private readonly FieldCollection fields;
        private readonly ILogEvents logger;
        private readonly IRuntimeContext context;

        public FieldCollectionWrapper(FieldCollection fieldCollection)
        {
            this.fields = fieldCollection;
        }

        public FieldCollectionWrapper(FieldCollection fieldCollection, IRuntimeContext context)
            : this(fieldCollection)
        {
            this.context = context;
            this.logger = context.Logger;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", Justification = "Available for mock testing only")]
        public IField this[string name]
        {
            get
            {
                return this.ApplyDoubleFix(this.fields[name]);
            }

            [EditorBrowsable(EditorBrowsableState.Never)]
            set
            {
                throw new InvalidOperationException("Only used for mocking from unit tests");
            }
        }

        public IEnumerator<IField> GetEnumerator()
        {
            return this.fields.Cast<Field>().Select(this.ApplyDoubleFix).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private IField ApplyDoubleFix(Field field)
        {
            IFieldExposed wrappedField = new FieldWrapper(field, this.context);
            wrappedField = new DoubleFixFieldDecorator(wrappedField, this.context);

            if (this.context.Settings.Debug)
            {
                wrappedField = new FieldValueValidationDecorator(wrappedField, this.context);
            }

            return wrappedField;
        }
    }
}
