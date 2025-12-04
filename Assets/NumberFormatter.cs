using UnityEngine;

public static class NumberFormatter
{
    static readonly string[] suffixes = 
    { 
        "", "K", "M", "B", "T", 
        "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ"
    };

    public static string FormatNumber(double value)
    {
        int index = 0;

        // 1000'den büyükse küçült ve suffix ekle
        while (value >= 1000 && index < suffixes.Length - 1)
        {
            value /= 1000f;
            index++;
        }

        // 2 basamak (1.53K gibi)
        return value.ToString("0.##") + suffixes[index];
    }
}
