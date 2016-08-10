namespace DDD.Infra.Validator
{
    public class ValidationResult
    {
        public string ErrorMessage { get; private set; }
        public string MemberNames { get; private set; }

        public ValidationResult(string errorMessage, string memberNames)
        {
            ErrorMessage = errorMessage;
            MemberNames = memberNames;
        }

        public override bool Equals(object obj)
        {
            var objeto = obj as ValidationResult;
            return objeto != null && objeto.ErrorMessage == ErrorMessage && objeto.MemberNames == MemberNames;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
