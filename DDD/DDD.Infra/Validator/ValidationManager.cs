using DDD.Infra.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DDD.Infra.Validator
{
    public class ValidationManager
    {
        public ValidationManager()
        {
            Messages = new HashSet<ValidationResult>();
        }

        private ISet<ValidationResult> Messages { get; set; }

        public ValidationManager GetMessages<TEntity>(TEntity item) where TEntity : class
        {
            var validator = EntityValidatorFactory.CreateValidator();
            this.AddMessages(validator.GetInvalidMessages(item));
            return this;
        }

        public ValidationManager AddMessages(IEnumerable<ValidationResult> messages)
        {
            this.Messages.UnionWith(messages);
            return this;
        }        

        public ValidationManager AddMessages(ValidationResult message)
        {
            this.Messages.Add(message);
            return this;
        }

        public ValidationManager AddMessages(string errorMessage, string memberNames)
        {
            return this.AddMessages(new ValidationResult(errorMessage, memberNames));
        }

        public ValidationManager AddMessages<TEntity, KProperty>(string errorMessage, Expression<Func<TEntity, KProperty>> propriedade)
        {
            return this.AddMessages(new ValidationResult(errorMessage, propriedade.Name));
        }

        public bool isValid()
        {
            return Messages.Count() == 0;
        }

        public void Validate()
        {
            if(!isValid()){
                throw new AppException(Messages);
            }
        }
    }
}
