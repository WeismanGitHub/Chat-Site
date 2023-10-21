namespace API;

internal static class Extensions {
    public static bool IsAValidPassword(this string password) {
        if (password == null) return false;

        bool meetsLengthRequirements = password.Length is >= User.MinPasswordLength and <= User.MaxPasswordLength;
        bool hasUpperCaseLetter = false;
        bool hasLowerCaseLetter = false;
        bool hasDecimalDigit = false;

        if (meetsLengthRequirements) {
            foreach (char c in password) {
                if (char.IsUpper(c)) hasUpperCaseLetter = true;
                else if (char.IsLower(c)) hasLowerCaseLetter = true;
                else if (char.IsDigit(c)) hasDecimalDigit = true;
            }
        }

        return meetsLengthRequirements &&
            hasUpperCaseLetter &&
            hasLowerCaseLetter &&
            hasDecimalDigit;
    }
}
