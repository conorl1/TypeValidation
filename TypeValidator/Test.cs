using TypeValidator;

class Test
{
    /* Test input types */
    class AddressInput(int? HouseNumber = null, string? Road = null, string? PostCode = null, string? Town = null, string? Borough = null)
    {
        public readonly int? HouseNumber = HouseNumber;
        public readonly string? Road = Road;
        public readonly string? Town = Town;
        public readonly string? Borough = Borough;
        public readonly string? PostCode = PostCode;
    }

    class PersonInput(string? Name = null, DateTime? DOB = null, AddressInput? Address = null)
    {
        public readonly string? Name = Name;
        public readonly DateTime? DOB = DOB;
        public readonly AddressInput? Address = Address;
    }

    /* Test valid types */
    class ValidAddress(int HouseNumber, string Road, string PostCode, string? Town = null, string? Borough = null)
    {
        public readonly int HouseNumber = HouseNumber;
        public readonly string Road = Road;
        public readonly string? Town = Town;
        public readonly string? Borough = Borough;
        public readonly string PostCode = PostCode;

        public override string ToString()
        {
            string output = HouseNumber.ToString() + " " + Road + ", ";

            if (Town != null)
            {
                output += Town + ", ";
            }

            if (Borough != null)
            {
                output += Borough + ", ";
            }

            return output + PostCode;
        }

    }

    class ValidPerson(string Name, DateTime DOB, ValidAddress Address)
    {
        public readonly string Name = Name;
        public readonly DateTime DOB = DOB;
        public readonly ValidAddress Address = Address;

        public override string ToString()
        {
            return "Name: " + Name + ", DOB: " + DOB.ToString() + ", Address: " + Address.ToString();
        }

    }

    /* Test validation functions */
    ValidationResult<object,string> ValidateName(PersonInput input)
    {
        if (input.Name == null || input.Name == "")
        {
            return new Error<object,string>(["Name must be entered"]);
        } else if (input.Name.Length > 64)
        {
            return new Error<object,string>(["Max length"]);
        }

        return new OK<object,string>(input.Name.Trim());
    }

    ValidationResult<object,string> ValidateDOB(PersonInput input)
    {
        if (input.DOB == null)
        {
            return new Error<object,string>(["DOB must be entered"]);
        } else if (input.DOB > DateTime.Today)
        {
            return new Error<object,string>(["Future date"]);
        } else if (input.DOB < new DateTime(1905,1,1))
        {
            return new Error<object,string>(["Before 1905"]);
        }

        return new OK<object,string>(input.DOB.Value);
    }

    ValidationResult<object,string> ValidateHouseNumber(AddressInput input)
    {
        if (input.HouseNumber == null)
        {
            return new Error<object,string>(["House number must be entered"]);
        } else if (input.HouseNumber < 1)
        {
            return new Error<object,string>(["House number must be 1 or greater"]);
        }

        return new OK<object,string>(input.HouseNumber.Value);
    }

    ValidationResult<object,string> ValidateRoad(AddressInput input)
    {
        if (input.Road == null || input.Road == "")
        {
            return new Error<object,string>(["Road must be entered"]);
        }

        return new OK<object,string>(input.Road);
    }

    ValidationResult<object,string> ValidatePostCode(AddressInput input)
    {
        if (input.PostCode == null || input.PostCode == "")
        {
            return new Error<object,string>(["Post code must be entered"]);
        } else if (input.PostCode.Length > 10)
        {
            return new Error<object,string>(["Post code too long"]);
        }

        return new OK<object,string>(input.PostCode);
    }

    ValidationResult<object,string> ValidateAddress(PersonInput input)
    {
        if (input.Address == null)
        {
            return new Error<object,string>(["Address must be entered"]);
        } else
        {
            var validator = new Validator<AddressInput,object,string>([ValidateHouseNumber,ValidateRoad,ValidatePostCode], AddressBuilder);
            return validator.Validate(input.Address);
        }
    }

    ValidationResult<object,string> ValidatePositiveInt(int? input)
    {
        if (input == null)
        {
            return new Error<object,string>(["Int must be entered"]);
        } else if (input.Value < 1)
        {
            return new Error<object,string>(["Int less than 1"]);
        }

        return new OK<object,string>(input.Value);
    }

    /* Test builder functions for building a valid type from an input type, used once it has been validated */
    ValidAddress AddressBuilder(AddressInput input)
    {
        return new ValidAddress(input.HouseNumber.Value,input.Road,input.PostCode,input.Town,input.Borough);
    }

    ValidPerson PersonBuilder(PersonInput input)
    {
        return new ValidPerson(input.Name,input.DOB.Value,AddressBuilder(input.Address));
    }

    /* Test code for running a Validator on a specific type */
    ValidationResult<ValidPerson,string> ValidatePerson(PersonInput person)
    {
        var validator = new Validator<PersonInput,ValidPerson,string>([ValidateName,ValidateDOB,ValidateAddress],PersonBuilder);
        return validator.Validate(person);
    }

    ValidationResult<int,string> IntValidator(int? integer)
    {
        var validator = new Validator<int?,int,string>([ValidatePositiveInt],i => i.Value);
        return validator.Validate(integer);
    }

    public static void Main(string[] args)
    {
        var test = new Test();

        Console.WriteLine(test.ValidatePerson(new PersonInput("John Smith",new DateTime(2001,3,12),new AddressInput(10, "A Road", "AB1 2CD", "Townham", "Broughborough"))).ToString());
        Console.WriteLine(test.ValidatePerson(new PersonInput(null,new DateTime(2001,3,12),new AddressInput(10, "A Road", "AB1 2CD", "Townham", "Broughborough"))).ToString());
        Console.WriteLine(test.ValidatePerson(new PersonInput("",new DateTime(2001,3,12),new AddressInput(10, "A Road", "AB1 2CD", "Townham", "Broughborough"))).ToString());
        Console.WriteLine(test.ValidatePerson(new PersonInput("John Smith",new DateTime(2030,3,12),new AddressInput(10, "A Road", "AB1 2CD", "Townham", "Broughborough"))).ToString());
        Console.WriteLine(test.ValidatePerson(new PersonInput("",new DateTime(2100,3,12),new AddressInput(10, "A Road", "AB1 2CD", "Townham", "Broughborough"))).ToString());
        Console.WriteLine(test.ValidatePerson(new PersonInput("John Smith",new DateTime(2001,3,12),new AddressInput(10, "", "AB1 2CD", "", ""))).ToString());
        Console.WriteLine(test.ValidatePerson(new PersonInput(null,new DateTime(1847,3,12),new AddressInput(null, null, "", "Townham", "Broughborough"))).ToString());
        
        Console.WriteLine(test.IntValidator(10));
        Console.WriteLine(test.IntValidator(-1));
    }

}

