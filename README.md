# Validation

This is a small C# library that allows a consumer to define validations for a generic type. 

Upon creating a Validator<TInput,TSuccess,TError> object, passing in a list of validation functions and a builder function to create the valid type from the input type, the method ValidationResult<TSuccess,TError> Validate(TInput item) can be called on the object to validate the input item and get the validated version of the input.

The file Test.cs contains a main method and example testing code.
