namespace Movies.Application.Common.Validators
{
    public static class CommonValidators
    {
        public static bool StartsWithUppercase(string genreName)
        {
            if (string.IsNullOrEmpty(genreName))
                return false;
            return char.IsUpper(genreName[0]);
        }
    }
}
