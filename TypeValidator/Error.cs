namespace TypeValidator;

/* Represents the result of an unsuccessful validation */
public class Error<TSuccess,TError>(List<TError> validations) : ValidationResult<TSuccess,TError>
{
    private readonly List<TError> validations = validations;

    public List<TError> GetErrors()
    {
        return validations;
    }

    public override string ToString()
    {
        var message = "Error: ";

        foreach (var validation in validations)
        {
            message += validation + ", ";
        }

        return message;
    }

}