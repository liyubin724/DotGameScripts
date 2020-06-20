namespace DotTool.ETD.Validation
{
    public class ErrorValidation : IFieldValidation
    {
        public string Rule { get; set; }

        public FieldValidationResult Verify()
        {
            throw new System.NotImplementedException();
        }
    }
}
