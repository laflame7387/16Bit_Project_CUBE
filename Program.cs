using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

                        default:
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
        Player player = new Player("Chad", "전사", 1, 100, 10);       //      이름, 직업, 레벨, 체력, 공격력

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
                StartBattle(player); // 여기에 보스 출력 로직을 넣어도 좋음
                Console.WriteLine("🎉 게임 클리어! 수고하셨습니다!");
                break;
            }
            else
            {
                StartBattle(player);       //      SB 메서드를 실행 및 player 정보를 전달
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

    static void StartBattle(Player player)
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

        List<Monster> monsterPool = new List<Monster>        //      몬스터 종류, 스탯 값
        {
            new Monster("미니언", 2, 15, 5),       //      이름, 레벨, 체력, 공격력
            new Monster("대포미니언", 5, 25, 10),
            new Monster("공허충", 3, 10, 13)
        };


        int beforeHP = player.CurrentHP;        //      전투 시작 전 현재 체력값 저장

        List<Monster> encountered = new List<Monster>();        //      컨택 시 List내 Monster 중 호출 <중복 가능성 있음>

        for (int i = 0; i < enemyCount; i++)      //      for 문으로 몬스터 생성
        {
            int index = rand.Next(monsterPool.Count);
            Monster m = new Monster(
                monsterPool[index].Name,
                monsterPool[index].Level,
                monsterPool[index].HP,
                monsterPool[index].Attack
                );
            encountered.Add(m);       //      컨택 시 "m" = monster 추가 <소환>
        }

        // 전투 반복 루프
        while (true)
        {
            foreach (var m in encountered)      //      몬스터 리스트를 하나씩 확인하며 출력
            {
                Console.WriteLine(m.ToString());
            }

            Console.WriteLine();        //      한줄 띄우기 용 <공백 칸>
            player.Showinfo();          //      플레이어 스탯 띄우는 칸 <체력 직업 등>

            Console.WriteLine("\n1. 공격");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            string input = Console.ReadLine();

            switch (input)
            {
                case ("1"):
                    BattleSystem.PlayerAttack(player, encountered);
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("잘못된 입력입니다");
                    Thread.Sleep(1500);
                    return;
            }

            // 승패 조건 확인 시스템
            if (player.CurrentHP <= 0 || encountered.All(m => m.HP <= 0))
                break;

            BattleSystem.EnemyPhase(player, encountered);        //      몬스터 턴

            if (player.CurrentHP <= 0 || encountered.All(m => m.HP <= 0))
                break;
        }
        BattleSystem.ShowBattleResult(player, encountered, beforeHP);
    
    }


    class Monster       //      몬스터 기본 틀
    {
        public string Name;     //      몬스터 이름
        public int Level;       //      몬스터 레벨
        public int HP;          //      몬스터 체력
        public int Attack;      //      몬스터 공격력

        public Monster(string name, int level, int hp, int attack)
        {
            Name = name;
            Level = level;
            HP = hp;
            Attack = attack;
        }

        public override string ToString()
        {
            return $"Lv.{Level} {Name} HP {HP}";
        }
    }

    class Player        //      플레이어 기본 틀
    {
        public string Name;     //      플레이어 이름
        public string Job;      //      플레이어 직업
        public int Level;       //      플레이어 레벨
        public int MaxHP;       //      플레이어 최대 체력
        public int CurrentHP;   //      플레이어 현재 체력
        public int Attack;      //      플레이어 공격력

        public Player(string name, string job, int level, int maxHP, int attack)
        {
            Name = name;
            Job = job;
            Level = level;
            MaxHP = maxHP;
            CurrentHP = maxHP;
            Attack = attack;
        }

        public static Player player { get; internal set; }

        public void Showinfo()      //      캐릭터 상태창 <조훈희님> / <에러 뜨는것이 거슬려 잠시 추가했습니다>
        {
            Console.WriteLine($"[캐릭터 정보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP {CurrentHP}/{MaxHP}");
        }
    }


    class BattleSystem      //      전투 시스템 틀
    {
        // 플레이어 공격 기능 시스템
        public static void PlayerAttack(Player player, List<Monster> monsters)
        {
            Console.Clear();
            Console.WriteLine($"Battle!!\n");

            for (int i = 0; i < monsters.Count; i++)
            {
                Monster m = monsters[i];
                Console.ForegroundColor = m.HP > 0 ? ConsoleColor.White : ConsoleColor.DarkGray;        //      몬스터 체력 0보다 이하 시 네임택 흰색에서 회색으로
                Console.WriteLine($"{i + 1} Lv.{m.Level} {m.Name}");
            }

            Console.ResetColor();
            Console.WriteLine();
            player.Showinfo();

            Console.WriteLine("\n0. 취소");
            Console.Write("\n대상을 선택해주세요.\n>> ");

            string input = Console.ReadLine();
            if (!int.TryParse(input, out int choice) || choice < 0 || choice > monsters.Count)     //      "일치하는 몬스터"를 선택하지 "않았을" 시
            {
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            if (choice == 0)
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                return;
            }

            Monster target = monsters[choice - 1];      //      플레이어가 선택한 몬스터를 "target" 이름으로 저장, index 번호를 맞추기 위해 -1 입력

            if (target.HP <= 0)     //      이미 죽은 몬스터 선택 시
            {
                Console.Clear();
                Console.WriteLine("잘못된 입력입니다.");
                Thread.Sleep(1000);
                return;
            }

            // 공격력 계산 식
            int baseDamage = player.Attack;     //      플레이어 기본 공격력 <상태창 완성 시 수정>
            double variance = Math.Ceiling(baseDamage * 0.1);       //      오차가 소수점일 시 올림 처리
            int min = baseDamage - (int)variance;       //      variance = 편차, 즉 - 1 + 1 variance 를 넣음으로 9~11 사이 랜덤 값이 결정됨
            int max = baseDamage + (int)variance;       //      variance = 편차, 즉 - 1 + 1 variance 를 넣음으로 9~11 사이 랜덤 값이 결정됨

            int finalDamage = new Random().Next(min, max + 1);      //      최종 데미지는 = 하한값과 상한값 중 랜덤. max + 1은 값 11까지 나타내기 위해
            int beforeHP = target.HP;

            target.HP -= finalDamage;       //      몬스터 체력에 최종 데미지 -
            if (target.HP <= 0) target.HP = 0;      //      만약 몬스터 체력 0보다 작거나 같다 = 몬스터 체력 0

            //  계산 결과 출력
            Console.WriteLine($"Chad 의 공격!");
            Console.WriteLine($"{target.Name} 을 맞췄습니다. [데미지 : {finalDamage}]");                         //  어우 겹겹이 쌓인게 거북칩도 아니고;
            Console.WriteLine($"\n{target.Name}\nHP {beforeHP} -> {(target.HP <= 0 ? "Dead" : target.HP.ToString())}");     //      몬스터의 맞기 전 체력 -> 맞은 후 [체력이 0 일 때: 참-Dead 출력 : 거짓-몬스터 체력 출력] <삼항 연산자>

            Console.Write(">> ");
            Console.ReadLine();

        }

        // 적 차례 공격 시스템
        public static void EnemyPhase(Player player, List<Monster> monsters)
        {
            Console.Clear();
            Console.WriteLine("Enemy Phase\n");

            foreach (Monster m in monsters)
            {
                if (m.HP <= 0)
                {
                    continue;       //      Dead 상태일 시 공격 하지 않는다
                }

                Console.WriteLine("Battle!!\n");

                Console.WriteLine($"Lv.{m.Level} {m.Name} 의 공격!");      //      m = monster

                // 몬스터 데미지 계산 식 +-10% 오차
                int baseDamage = m.Attack;
                double variance = Math.Ceiling(baseDamage * 0.1);       //      오차가 소수점일 시 올림 처리 
                int min = baseDamage - (int)variance;
                int max = baseDamage + (int)variance;

                int finalDamage = new Random().Next(min, max + 1);
                int beforeHP = player.CurrentHP;

                player.CurrentHP -= finalDamage;        //      플레이어 현재 체력에 몬스터의 최종 데미지 -
                if (player.CurrentHP <= 0) player.CurrentHP = 0;

                Console.WriteLine($"{player.Name}을 맞췄습니다. [데미지 : {finalDamage}]\n");

                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {beforeHP} -> {player.CurrentHP}\n");        //      플레이어 몬스터에게 피격 전 체력 -> 피격 후 체력

                Console.WriteLine("대상을 선택해주세요.\n>> ");
                Console.ReadLine();
                Console.Clear();
            }

            Console.WriteLine("EnemyPhase Phase 종료. 플레이어 턴으로 돌아갑니다.");
            Console.ReadLine();
        }

        // 전투 결과 출력 시스템
        public static void ShowBattleResult(Player player, List<Monster> monsters, int beforeHP)
        {
            Console.Clear();
            Console.WriteLine("Battle - Result\n");

            if (player.CurrentHP <= 0)      //      만약 플레이어 체력이 0보다 작거나 같으면 밑 콘솔 출력
            {
                Console.WriteLine("You Lose\n");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {beforeHP} -> 0\n");
            }
            else       //       아닐 시 밑 기능 실행 
            {
                Console.WriteLine("Victory\n");

                int defeated = 0;       //      기본 값 0
                foreach (var m in monsters)
                {
                    if (m.HP <= 0) defeated++;      //      몬스터의 체력이 0보다 작거나 같을 시 defeated에 1씩 추가
                }

                Console.WriteLine($"던전에서 몬스터 {defeated}마리를 잡았습니다.\n");
                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {beforeHP} -> {player.CurrentHP}\n");
            }
            Console.WriteLine("0 다음");
            Console.WriteLine(">> ");
            Console.ReadLine();
        }
    };

}