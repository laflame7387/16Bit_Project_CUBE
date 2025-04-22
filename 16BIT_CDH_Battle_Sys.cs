namespace _16BIT_Project
{
    using System;
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

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


        public void Showinfo()      //      캐릭터 상태창 <조훈희님> / <에러 뜨는것이 거슬려 잠시 추가했습니다>
        {
            Console.WriteLine($"[캐릭터 정보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP {CurrentHP}/{MaxHP}");
        }
    }


    class BattleSystem      //      전투 시스템 틀
    {

        static Random random = new Random();

        static List<Monster> monsterPool = new List<Monster>        //      몬스터 종류, 스탯 값
        {
            new Monster("미니언", 2, 15, 5),       //      이름, 레벨, 체력, 공격력
            new Monster("대포미니언", 5, 25, 10),
            new Monster("공허충", 3, 10, 13)
        };

        public static void StartBattle(Player player)       //      전투 시작시
        {
            Console.WriteLine("Battle!!\n");

            int beforeHP = player.CurrentHP;        //      전투 시작 전 현재 체력값 저장

            int monsterCount = random.Next(1, 5);       //      1~4 마리 스폰 설정
            List<Monster> encountered = new List<Monster>();        //      컨택 시 List내 Monster 중 호출 <중복 가능성 있음>

            for (int i = 0; i < monsterCount; i++)      //      for 문으로 몬스터 생성
            {
                int index = random.Next(monsterPool.Count);
                Monster m = new Monster(
                    monsterPool[index].Name,
                    monsterPool[index].Level,
                    monsterPool[index].HP,
                    monsterPool[index].Attack
                    );
                encountered.Add( m );       //      컨택 시 "m" = monster 추가 <소환>
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

                if (input == "1")
                {
                    PlayerAttack(player, encountered);
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다");
                    continue;
                }

                // 승패 조건 확인 시스템
                if (player.CurrentHP <= 0 || encountered.All(m => m.HP <= 0))
                        break;

                EnemyPhase(player, encountered);        //      몬스터 턴

                if (player.CurrentHP <= 0 || encountered.All(m => m.HP <= 0))
                        break;
            }

            ShowBattleResult(player, encountered, beforeHP);
        }

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
                Console.WriteLine("잘못된 입력입니다.");
                return;
            }

            Monster target = monsters[choice - 1];      //      플레이어가 선택한 몬스터를 "target" 이름으로 저장, index 번호를 맞추기 위해 -1 입력

            if (target.HP <= 0)     //      이미 죽은 몬스터 선택 시
            {
                Console.WriteLine("잘못된 입력입니다.");
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
            Console.WriteLine("\n0. 다음\n");
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

                

                int finalDamage = new Random().Next(max, min + 1);
                int finalDamage = new Random().Next(min, max + 1);
                int beforeHP = player.CurrentHP;

                player.CurrentHP -= finalDamage;        //      플레이어 현재 체력에 몬스터의 최종 데미지 -
                if (player.CurrentHP <= 0) player.CurrentHP = 0;

                Console.WriteLine($"{player.Name}을 맞췄습니다. [데미지 : {finalDamage}]\n");

                Console.WriteLine($"Lv.{player.Level} {player.Name}");
                Console.WriteLine($"HP {beforeHP} -> {player.CurrentHP}\n");        //      플레이어 몬스터에게 피격 전 체력 -> 피격 후 체력

                Console.WriteLine("0. 다음");
                Console.WriteLine("대상을 선택해주세요.\n>> ");
                Console.ReadLine();
                Console.Clear();
            }

            Console.WriteLine("EnemyPhase Phase 종료. 플레이어 턴으로 돌아갑니다.");
            Console.WriteLine("0. 다음");
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
                foreach (var m in monsters)      //     ?? 설명 필요
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

   


    internal class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player("Chad", "전사", 1, 100, 10);       //      이름, 직업, 레벨, 체력, 공격력
            BattleSystem.StartBattle(player);       //      전투 시작 시 BS 클래스의 SB 메서드를 실행 및 player 정보를 전달
            Console.ReadLine();
        }
    }
}
