using System;
using System.Collections.Generic;
using System.Threading;

class Player
{
    public string Name = "플레이어";
    public string Job = "전사";
    public int Level = 1;
    public int Exp = 0;
    public int ExpToLevel = 100;
    public int MaxExp = 100;
    public int MaxHP = 100;
    public int HP = 100;
    public int Atk = 10;
    public int Def = 10;

    public string Weapon = "없음";
    public int WeaponAtk = 0;
    public string Armor = "없음";
    public int ArmorDef = 0;

    public int TotalAtk => Atk + WeaponAtk;
    public int TotalDef => Def + ArmorDef;

    public void DisplayStat()
    {
        Console.Clear();
        Console.WriteLine($"상태창");
        Console.WriteLine($"이름: {Name} ({Job})");
        Console.WriteLine($"레벨: {Level}");
        Console.WriteLine($"체력: {HP}");
        Console.WriteLine($"공격력: {Atk} ({WeaponAtk}) = {TotalAtk}");
        Console.WriteLine($"방어력: {Def} ({ArmorDef}) = {TotalDef}");
        Console.WriteLine("무기: {Weapon}, 방어구: {Armor}");
        Console.WriteLine($"경험치: {Exp}/{ExpToLevel}");
        Console.WriteLine("\n 0을 눌러 메뉴로 돌아가기");
        Console.ReadLine();
    }
    public void GainExp(int amount)
    {
        Exp += amount;
        Console.WriteLine($"경험치 {amount} 획득!");

        while (Exp >= ExpToLevel)
        {
            Exp -= ExpToLevel;
            Level++;
            ExpToLevel += 50;
            MaxHP += 100;
            Atk += 2;
            Def += 1;
            HP = MaxHP;
            Console.WriteLine($"레벨업! 현재 레벨: {Level}");
        }
    }
}
class Program
{
    const int width = 12;
    const int height = 5;
    const char playerSymbol = '@';
    const char enemySymbol = 'E';

    static int currentFloor = 1;
    static int playerLevel = 1;
    static int playerHP = 100;
    static int playerAtk = 10;

    static string select;
    static int answer;
    static bool check;

    static Player player = new Player();
    static void Main()
    {
        string input;

       
        while (true)
        {
            Console.Clear();
            Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
            Console.WriteLine("이제 전투를 시작할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 전투시작");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            input = Console.ReadLine();
            Console.Clear();

            if (input == "0")
                break;

            switch (input)
            {
                case "1":
                    Console.WriteLine("1. 상태보기");
                    player.DisplayStat();
                    break;
                case "2":
                    Console.WriteLine("2. 전투시작");
                    StartDungeon();
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }

        static void StartDungeon()
    {
        while (currentFloor <= 16)
        {
            Console.Clear();
            Console.WriteLine($"📦 현재 층: {currentFloor}층");

            if (currentFloor % 3 == 0 && currentFloor != 0 && currentFloor < 16)
            {
                ShowRestStage();
            }
            else if (currentFloor == 16)
            {
                Console.WriteLine("🏆 최종 보스층에 도달했습니다!");
                StartBattle(); // 여기에 보스 출력 로직을 넣어도 좋음
                Console.WriteLine("🎉 게임 클리어! 수고하셨습니다!");
                break;
            }
            else
            {
                StartBattle();
                Console.WriteLine("\n다음 층으로 이동하려면 Enter를 누르세요...");
                Console.ReadLine();
                currentFloor++;
            }
        }
    }

    static void ShowRestStage()
    {
        Console.WriteLine("🛏 큐브에서 안전한 장소를 찾았습니다... 체력 회복 및 레벨업이 가능합니다.\n");
        Console.WriteLine("1. 체력 완전 회복");
        Console.WriteLine("2. 레벨업 (+공격력 증가)");
        Console.WriteLine("3. 스킵");

        Console.Write("\n>> 선택: ");
        string? input = Console.ReadLine();

        switch (input)
        {
            case "1":
                playerHP = 100;
                Console.WriteLine("체력이 완전히 회복되었습니다!");
                break;
            case "2":
                playerLevel++;
                playerAtk += 3;
                Console.WriteLine($"레벨이 {playerLevel}로 올랐습니다! 공격력이 {playerAtk}로 증가!");
                break;
            case "3":
                Console.WriteLine("휴식을 건너뛰고 바로 진행합니다.");
                break;
            default:
                Console.WriteLine("잘못된 입력입니다. 다시 선택해주세요.");
                ShowRestStage();
                return;
        }

        Console.WriteLine("\nEnter를 눌러 다음 층으로 이동...");
        Console.ReadLine();
        currentFloor++;
    }

    static void StartBattle()
    {
        Console.Clear();
        Console.WriteLine($"큐브 ⚔ {currentFloor}층... 전투에 돌입합니다!\n");

        char[,] room = new char[height, width];

        // 전체를 공백으로 초기화
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                room[y, x] = ' ';

        // 플레이어 위치 고정
        int playerX = 3;
        int playerY = 2;
        room[playerY, playerX] = playerSymbol;

        // 적 개수 랜덤 (1~4명)
        Random rand = new Random();
        int enemyCount = rand.Next(1, 5);

        HashSet<string> used = new HashSet<string>();
        used.Add($"{playerX},{playerY}");

        for (int i = 0; i < enemyCount; i++)
        {
            int ex, ey;
            do
            {
                ex = rand.Next(1, width - 1);
                ey = rand.Next(1, height - 1);
            } while (used.Contains($"{ex},{ey}"));

            room[ey, ex] = enemySymbol;
            used.Add($"{ex},{ey}");
        }

        // 테두리 포함하여 출력
        for (int y = 0; y < height; y++)
        {
            string line = "";
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || x == width - 1)
                    line += "|";
                else if (y == 0 || y == height - 1)
                    line += "-";
                else
                    line += room[y, x];
            }
            Console.WriteLine(line);
        }

        Console.WriteLine($"\n출현한 적 수: {enemyCount}");
        Console.WriteLine($"플레이어 공격력: {playerAtk}, 현재 체력: {playerHP}");

        player.GainExp(enemyCount * 30);
    }
}
