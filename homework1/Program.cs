using System;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

Console.OutputEncoding = Encoding.GetEncoding(866);
Console.InputEncoding = Encoding.GetEncoding(866);

Console.WriteLine("Реализация алгоритма поиска с опечатками");
Console.WriteLine("Программа вычисления расстояния Левенштейна");
Console.WriteLine("Для выхода введите 'exit' в качестве первой строки\n");

while (true)
{
    Console.Write("Введите первую строку: ");
    string str1 = Console.ReadLine();

    if (str1?.ToLower() == "exit")
    {
        Console.WriteLine("Программа завершена.");
        break;
    }

    Console.Write("Введите вторую строку: ");
    string str2 = Console.ReadLine();

    int distance = LevenshteinDistance(str1, str2);

    Console.WriteLine($"Расстояние Левенштейна между '{str1}' и '{str2}': {distance}\n");
}

static int LevenshteinDistance(string str1Param, string str2Param)
{
    if ((str1Param == null) || (str2Param == null))
        return -1;

    int str1Len = str1Param.Length;
    int str2Len = str2Param.Length;

    if ((str1Len == 0) && (str2Len == 0))
        return 0;

    if (str1Len == 0)
        return str2Len;

    if (str2Len == 0)
        return str1Len;

    string str1 = str1Param.ToUpper();
    string str2 = str2Param.ToUpper();

    int[,] matrix = new int[str1Len + 1, str2Len + 1];

    for (int i = 0; i <= str1Len; i++)
        matrix[i, 0] = i;

    for (int j = 0; j <= str2Len; j++)
        matrix[0, j] = j;

    for (int i = 1; i <= str1Len; i++)
    {
        for (int j = 1; j <= str2Len; j++)
        {
            int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;

            int deletion = matrix[i - 1, j] + 1;
            int insertion = matrix[i, j - 1] + 1;
            int substitution = matrix[i - 1, j - 1] + cost;

            matrix[i, j] = Math.Min(Math.Min(deletion, insertion), substitution);
        }
    }
    return matrix[str1Len, str2Len];
}