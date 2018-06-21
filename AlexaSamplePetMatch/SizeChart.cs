using System;
using System.Collections.Generic;
using System.Text;

namespace AlexaSamplePetMatch
{
    public static class SizeChart
    {
        public static Dictionary<string, DogSize> Sizes = new Dictionary<string, DogSize>
        {
            {"tiny", new DogSize("4 to 6", "1.8 to 2.7")},
            {"small", new DogSize("7 to 20", "3.18 to 9")},
            {"medium", new DogSize("21 to 54", "9.53 to 24.49")},
            {"large", new DogSize("55 to 80", "24.94 to 38.28")}
        };
    }

public class DogSize
    {
        public string Pounds { get; }
        public string Kilograms { get; set; }

        public DogSize(string pounds, string kilograms)
        {
            Pounds = pounds;
            Kilograms = kilograms;
        }

    }
}
