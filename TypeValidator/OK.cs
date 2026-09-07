namespace TypeValidator;

/* Represents the result of a successful validation */
public class OK<TSuccess,TError>(TSuccess convertedInput) : ValidationResult<TSuccess,TError>
{
    private readonly TSuccess convertedInput = convertedInput;

    public TSuccess GetValidatedInput()
    {
        return convertedInput;
    }

    public override string ToString()
    {
        return "OK: " + convertedInput.ToString();
    }

}