namespace GB.ASPNET.WebStore.Models
{
    public static class General
    {
        public static string AgeUnit(int age)
        {
            if (10 < age && age < 15) return "лет";
            else
            {
                int lastDigit = age % 10;
                if (lastDigit == 1) return "год";
                else if (lastDigit > 1 && lastDigit < 5) return "года";
                else return "лет";
            }
        }
    }
}
