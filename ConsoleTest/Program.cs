
using System.Numerics;
using System.Text;

Random rnd = new Random();

GenerateFile("D:\\test.txt", 100000);


void GenerateFile(string ouputPath, int stringsAmount)
{
    using (StreamWriter writer = new StreamWriter(ouputPath, false))
    {
        writer.WriteLine(stringsAmount);
        for (int i = 0; i < stringsAmount; i++)
        {
            BigInteger first = GenerateBigInt(MinLength: 500, Maxlength: 1000);


            BigInteger second = GenerateBigInt(MinLength: 500, Maxlength: 1000);

            string line = first.ToString() + " "+GenerateSign()+ " " + second.ToString();
            
            writer.WriteLine(line);
        }
        
    }
}

BigInteger GenerateBigInt(int MinLength, int Maxlength)
{
    int length = rnd.Next(MinLength, Maxlength);
    StringBuilder str = new StringBuilder();
    for (int i = 0; i < length; i++)
    {
       str.Append(rnd.Next(10));
    }
    return BigInteger.Parse(str.ToString());
}

string GenerateSign()
{
    switch (rnd.Next(4))
    {
        case 0: return "+";
        case 1: return "-";
        case 2: return "*";
        case 3: return "/";
        default: return "=";
    }
}