using System;
using System.Collections.Generic;

class BattleRoom
{
    const int width = 12;
    const int height = 5;
    const char playerSymbol = '@';
    const char enemySymbol = 'E';

    static void Main()
    {
        StartBattle();
    }

    static void StartBattle()
    {
        Console.Clear();
        Console.WriteLine("⚔ 전투에 돌입합니다!\n");

        char[,] room = new char[height, width];

        // 전체를 공백으로 초기화
        for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                room[y, x] = ' ';

        // 플레이어 위치 고정 (예: 2, 3)
        int playerX = 3;
        int playerY = 2;
        room[playerY, playerX] = playerSymbol;

        // 적 개수 랜덤 (1~4명)
        Random rand = new Random();
        int enemyCount = rand.Next(1, 5);

        HashSet<string> used = new HashSet<string>();
        used.Add($"{playerX},{playerY}"); // 플레이어 위치 차지했으니 중복 방지

        for (int i = 0; i < enemyCount; i++)
        {
            int ex, ey;
            do
            {
                ex = rand.Next(1, width - 1);  // 테두리 안쪽
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
                if (x == 0 && y == 0)
                    line += "+";
                else if (x == 0 && y == height - 1)
                    line += "+";
                else if (x == 0 || x == width - 1)
                    line += "|";
                else if (y == 0 || y == height - 1)
                    line += "-";
                else
                    line += room[y, x];
            }
            Console.WriteLine(line);
        }

        Console.WriteLine($"\n출현한 적 수: {enemyCount}");
    }
}
