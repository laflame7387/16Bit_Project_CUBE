using System;
using System.Collections.Generic;
using System.Threading;

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
    static void Main()
    {
        do
        {
            Console.WriteLine("테스트할 영역을 선택해주세요.");
            Console.WriteLine();
            Console.WriteLine("1. 던전파트");
            Console.WriteLine("프로그램 종료 = 0");
            select = Console.ReadLine();
            check = int.TryParse(select, out answer);

            if (!check)
            {
                Console.Clear();
                continue;
            }
            else
            {
                if (answer == 0)
                {
                    break;
                }
                else if (answer < 0)
                {
                    Console.Clear();
                    continue;
                }
                else
                {
                    switch (answer)
                    {
                        case 1:
                            {
                                StartDungeon();
                            }
                            break;
                        
                        default :
                            {
                                break;
                            }
                    }
                }
            }
            
            Console.Clear();
        }
        while (true);// 프로젝트 반복문
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
        Console.WriteLine("🛏 휴식 스테이지입니다. 체력 회복 및 레벨업이 가능합니다.\n");
        Console.WriteLine("1. 체력 완전 회복");
        Console.WriteLine("2. 레벨업 (+공격력 증가)");
        Console.WriteLine("3. 그냥 다음 층으로");

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
        Console.WriteLine($"⚔ {currentFloor}층 전투에 돌입합니다!\n");

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
    }

    static void inventory()
    {
        
        Console.Clear();
        Console.WriteLine("인벤토리");
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("[아이템 목록]");
        Console.WriteLine();
        Console.WriteLine("무기 1");
        Console.WriteLine("무기 1");
        Console.WriteLine("무기 1");
        Console.WriteLine("방어구 1");
        Console.WriteLine("방어구 1");
        Console.WriteLine("방어구 1");
        Console.WriteLine("포션");
        
    }
}
