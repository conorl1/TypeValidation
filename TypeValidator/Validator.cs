namespace TypeValidator;

/* A Validator can be used to validate any input type */
public class Validator<TInput,TSuccess,TError>
{
    /* The function to validate an input */
    private Func<TInput,ValidationResult<TSuccess,TError>> validationRoutine;

    public Validator(Func<TInput,ValidationResult<object,TError>>[] validations, Func<TInput,TSuccess> builder)
    {
        validationRoutine = CombineValidations(validations, builder);
    }

    /* Combine input validation functions together */
    private static Func<TInput,ValidationResult<TSuccess,TError>> CombineValidations(Func<TInput,ValidationResult<object,TError>>[] validations, Func<TInput,TSuccess> builder)
    {
        return input =>
        {
            List<TError> errors = [];
            List<object> successes = [];

            foreach (var validation in validations)
            {
                var result = validation(input);

                if (result is Error<object,TError> error)
                {
                    errors.AddRange(error.GetErrors());
                }
                else if (result is OK<object,TError> ok) {
                    successes.Add(ok.GetValidatedInput());
                }
                
            }

            if (errors.Count > 0)
            {
                return new Error<TSuccess,TError>(errors);
            } else
            {
                return new OK<TSuccess,TError>(builder(input));
            }

        };
    }

    /* Run the validation routine on the input */
    public ValidationResult<TSuccess,TError> Validate(TInput item)
    {
        return validationRoutine(item);
    }

}


