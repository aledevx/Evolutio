using Sqids;

namespace CommonTestUtilities.IdEncryption;
public class IdEncripterBuilder
{
    public static SqidsEncoder<long> Build()
    {
        var configOptions = new SqidsEncoder<long>(new SqidsOptions() 
        {
            MinLength = 3,
            Alphabet = "VWdeDEFGHIJKfghijnopqr3456stuvwxyzABCLMNklmOPQRSTUXYZ0abc12789",
        });

        return configOptions;
    }
}

