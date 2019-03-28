using System.Text;
using System.Security.Cryptography;
using System;
using System.Security;

class Block
{
    public int Index { get; set; }
    public DateTime TimeStamp { get; set; }
    public String Data { get; set; }
    public String PreviousHash { get; set; }
    public String CurrentHash { get; set; }

    public Block(DateTime timeStamp, String previousHash, String data)
    {
        Index = 0;
        TimeStamp = timeStamp;
        PreviousHash = previousHash;
        Data = data;
        CurrentHash = CalculateHash();
    }

    //블록내의 멤버들을 연결하여 해쉬함수를 계산.
    public String CalculateHash()
    {
        //Hash함수 SHA256을 사용
        SHA256 sha256 = SHA256.Create();

        //메모리최적화를 위해 StringBuilder 사용 후 필요멤버 추가
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append($"{ TimeStamp }");
        stringBuilder.Append($"{ Data }");
        stringBuilder.Append($"{ PreviousHash ?? "" }");


        //sha256 생성
        byte[] inputDatas = Encoding.UTF8.GetBytes(stringBuilder.ToString());
        byte[] outputDatas = sha256.ComputeHash(inputDatas);

        //생성된 Byte[]를 String값으로 변경 -Hex형식
        return BitConverter.ToString(outputDatas).Replace("-", String.Empty);
    }
}