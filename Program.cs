using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks.Dataflow;

class Player
{
    public string? Name;
    public string Job;
    public int Level;
    public int Exp;
    public int ExpToLevel;
    public int MaxExp;
    public int MaxHP;
    public int HP;
    public int Atk;
    public float CritChance;
    public float CritMultiplier;
    public int Def;

    public string Weapon = "없음";
    public int WeaponAtk = 0;
    public string Armor = "없음";
    public int ArmorDef = 0;

    public int TotalAtk => Atk + WeaponAtk;
    public int TotalDef => Def + ArmorDef;

    public void DisplayStat()
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"상태창");
            Console.ResetColor();
            Console.WriteLine($"이름: {Name} ({Job})");
            Console.WriteLine($"레벨: {Level}");
            Console.WriteLine($"체력: {HP}");
            Console.WriteLine($"공격력: {Atk} ({WeaponAtk}) = {TotalAtk}");
            Console.WriteLine($"방어력: {Def} ({ArmorDef}) = {TotalDef}");
            Console.WriteLine("무기: {Weapon}, 방어구: {Armor}");
            Console.WriteLine($"경험치: {Exp}/{ExpToLevel}");

            Console.WriteLine("\n\"0\"을 눌러 메뉴로 돌아가기");
            string input = Console.ReadLine();

            if (input == "0")
            {
                break;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
                Thread.Sleep(1000);
            }
        }
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

    // 캐릭터 직업 선택, 생성
    public static Player CreatePlayer()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("큐브의 미궁에 사로잡힌 당신은 탈출구를 찾고자 나아가기 시작합니다..."); //마침표 3개로 고쳤어요 !
        Console.ResetColor();
        Thread.Sleep(2000);
        Console.Clear();
        Console.WriteLine("당신의 이름은 무엇입니까");
        string? name = Console.ReadLine();

        Console.WriteLine("\n당신의 직업은 무엇입니까");
        Thread.Sleep(1000);
        Console.WriteLine("1. 전사");
        Console.WriteLine("2. 도적");
        Console.WriteLine("3. 기사");

        string jobinput = Console.ReadLine();
        string job = "";
        int level = 0;
        int exp = 0;
        int expToLevel = 0;
        int maxExp = 0;
        int maxHP = 0;
        int hp = 0;
        int atk = 0;
        float critchance = 0f;
        float critmultiplier = 0f;
        int def = 0;

        switch (jobinput)
        {
            case "1":
                job = "전사";
                level = 1;
                exp = 0;
                expToLevel = 100;
                maxExp = 100;
                maxHP = 100;
                hp = 100;
                atk = 10;
                critchance = 0.15f;
                critmultiplier = 1.6f;
                def = 10;
                break;
            case "2":
                job = "도적";
                level = 1;
                exp = 0;
                expToLevel = 100;
                maxExp = 100;
                maxHP = 90;
                hp = 90;
                atk = 13;
                critchance = 0.23f;
                critmultiplier = 1.8f;
                def = 8;
                break;
            case "3":
                job = "기사";
                level = 1;
                exp = 0;
                expToLevel = 100;
                maxExp = 100;
                maxHP = 130;
                hp = 130;
                atk = 9;
                critchance = 0.1f;
                critmultiplier = 1.3f;
                def = 13;
                break;
            default:
                job = "형태 없는 자";
                level = 9999;
                exp = 0;
                expToLevel = 9999;
                maxExp = 9999;
                maxHP = 99999;
                hp = 99999;
                atk = 9999;
                critchance = 90f;
                critmultiplier = 2f;
                def = 9999;
                break;
        }

        Player newChar = new Player
        {
            Name = name,
            Job = job,
            Level = level,
            Exp = exp,
            ExpToLevel = expToLevel,
            MaxExp = maxExp,
            MaxHP = maxHP,
            HP = hp,
            Atk = atk,
            CritChance = critchance,
            CritMultiplier = critmultiplier,
            Def = def,
        };

        Console.WriteLine($"\n{name} 의 직업은 {job} 입니다.");
        Console.WriteLine($"HP: {maxHP}, ATK: {atk}, DEF: {def}  LV: {level}");
        Console.ReadKey();

        return newChar;
    }
}
public class Item//아이템 가상메서드 이용
{
    public string itemName;
    public int itemAtk;
    public int itemDef;
    public string itemType;
    public int itemCount;

    public Item(string itemName, int itemAtk, int itemDef, int itemCount, string itemType)//아이템 기본 생성자
    {
        this.itemName = itemName;
        this.itemAtk = itemAtk;
        this.itemDef = itemDef;
        this.itemType = itemType;
        this.itemCount = itemCount;
    }
    public void SetItemInfo(string itemName, int itemAtk, int itemDef, int itemCount, string itemType)
    {
        this.itemName = itemName;
        this.itemAtk = itemAtk;
        this.itemDef = itemDef;
        this.itemType = itemType;
        this.itemCount = itemCount;
    }
    public virtual void Use()
    {
        Console.WriteLine($"{itemName}을 사용했습니다.");
    }
    public virtual void UnEquip()
    {
        Console.WriteLine("장비를 해제했습니다.");
    }

    public virtual bool IsEquip => false; //장비 장착 확인
}
public class Weapon : Item //무기
{
    public bool isEquipped;

    public Weapon(string itemName, int itemAtk) : base(itemName, itemAtk, 0, 0,"무기") 
    {
        isEquipped = false;
    }

    public override void Use()
    {
        if (!isEquipped)
        {
            isEquipped = true;
            Console.WriteLine($"{itemName} 을(를) 장착했습니다. 공격력이 {itemAtk} 증가합니다.");
        }    
    }
    public override void UnEquip()
    {
        if(isEquipped)
        {
            isEquipped = false;
            Console.WriteLine($"{itemName} 을(를) 해제했습니다.");
        }
    }

    public override bool IsEquip => isEquipped;
}
public class Armor : Item //방어구
{
    public bool isEquipped;

    public Armor(string itemName, int itemDef) : base(itemName, 0, itemDef, 0, "방어구") 
    {
        isEquipped = false;
    }

    public override void Use()
    {
        if (!isEquipped)
        {
            isEquipped = true;
            Console.WriteLine($"{itemName} 방어구를 장착했습니다. 방어력이 {itemDef} 증가합니다.");
        }
    }
    public override void UnEquip()
    {
        if (isEquipped)
        {
            isEquipped = false;
            Console.WriteLine($"{itemName} 을(를) 해제했습니다.");
        }

    public override bool IsEquip => isEquipped;
}
public class Supplies : Item //소모품 
{
    public Supplies(string itemName, int itemCount) : base(itemName, 0, 0, itemCount, "소모품") { }

    public override void Use()
    {
        if (itemCount > 0)
        {
            itemCount--;
            Console.WriteLine($"{itemName}을(를) 사용했습니다. (남은 수량: {itemCount})");
        }
        else
        {
            itemCount = 0;
            Console.WriteLine($"{itemName}이(가) 더 이상 남아있지 않습니다.");
        }
    }
}
public class Others : Item //기타 아이템
{
    public Others(string itemName, int itemCount) : base(itemName, 0, 0, itemCount, "기타") { }

    public override void Use()
    {
        Console.WriteLine($"{itemName}을(를) 사용했지만 특별한 효과는 없습니다.");
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

        player = Player.CreatePlayer();

        List<Item> inven = new List<Item>();// 샘플 아이템
        inven.Add(new Weapon("낡은 검", 3));
        inven.Add(new Armor("녹슨 갑옷", 3));
        inven.Add(new Supplies("힐링 포션", 3));
        inven.Add(new Others("큐브 조각", 1));

        while (true)
        {
            Console.Clear();
            Console.WriteLine("이제 전투를 시작할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 전투시작");
            Console.WriteLine("3. 인벤토리");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(">> ");
            Console.ResetColor();

            input = Console.ReadLine();
            Console.Clear();

            if (input == "0")
                break;

            switch (input)
            {
                case "1":
                    player.DisplayStat();
                    break;
                case "2":                   
                    StartDungeon();
                    break;
                case "3":
                    Inventory(inven);
                    break;
                default:
                    Console.WriteLine("잘못된 입력입니다.");
                    break;
            }
        }
    }

    static void Inventory(List<Item> inven)//인벤토리
    {
        string input;
        do
        {
            Console.Clear();
            Console.WriteLine("[인벤토리]\n");
            Console.WriteLine("-[아이템 목록]-\n");
            
            if(inven.Count == 0)
            {
                Console.WriteLine("소지중인 아이템이 없습니다.");
            }
            
            else if (inven.Count > 0)
            {
                for(int i = 0; i < inven.Count; i++)
                {

                    Item item = inven[i];


                    string equipStatus = item.IsEquip ? "[E]" : "";
                    
                    if (inven[i].itemType == "무기")
                    {
                        if (item.IsEquip == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{i + 1}. {equipStatus}[{inven[i].itemType}] {inven[i].itemName} (공격력 +{inven[i].itemAtk})");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"{i + 1}. {equipStatus}[{inven[i].itemType}] {inven[i].itemName} (공격력 +{inven[i].itemAtk})");
                        } 
                    }
                    else if (inven[i].itemType == "방어구")
                    {
                        Console.WriteLine($"{i + 1}. [{inven[i].itemType}] {inven[i].itemName} (방어력 +{inven[i].itemDef})");
                    }
                    else
                    {
                        Console.WriteLine($"{i + 1}. [{inven[i].itemType}] {inven[i].itemName} (수량 : {inven[i].itemCount})");
                    }
                }
            }
            
            Console.WriteLine("0. 나가기");
            Console.WriteLine("\n원하는 행동을 입력해주세요.");
            Console.Write(">>");
            input = Console.ReadLine();

            if (int.TryParse(input, out int index) && index > 0 && index <= inven.Count)
            {
                ItemMenu(inven[index - 1]);
            }

        }
        while(input != "0");
        
    }   
    
    static void ItemMenu(Item item)// 아이템 상세 메뉴
    {
        string input;
        do
        {
            Console.Clear();
            Console.WriteLine($"[{item.itemName}] 상세 메뉴");
            Console.WriteLine("1. 아이템 사용");
            Console.WriteLine("2. 아이템 정보 보기");
            Console.WriteLine("0. 뒤로 가기");
            Console.Write(">> ");
            input = Console.ReadLine();
            switch(input)
            {
                case "0":
                    {
                        break;
                    }
                case "1":
                    {
                       if(!item.IsEquip)
                        {
                            item.Use();
                        }
                       else
                        {
                            item.UnEquip();
                        }
                            Console.WriteLine("\n아무 키나 눌러서 계속");
                        Console.ReadKey();
                        break;
                    }
                case "2":
                    {
                        Console.WriteLine($"\n이름: {item.itemName}");
                        Console.WriteLine($"타입: {item.itemType}");
                        Console.WriteLine($"공격력: {item.itemAtk}, 방어력: {item.itemDef}");
                        Console.WriteLine($"수량: {item.itemCount}");
                        Console.WriteLine("\n아무 키나 눌러서 계속");
                        Console.ReadKey();

                        break;
                    }
                default:
                    {
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                    }
            }
            
        }
        while (input != "0");
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
                Console.WriteLine("🏆 큐브의 끝자락, 16층에 도달했습니다!");
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
        Console.WriteLine("🛏 미궁에서 안전한 장소를 찾았습니다. 체력 회복 및 레벨업이 가능합니다.\n");
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

    // 전체 방을 공백으로 초기화
    for (int y = 0; y < height; y++)
        for (int x = 0; x < width; x++)
            room[y, x] = ' ';

    // 플레이어 위치
    int playerX = 3;
    int playerY = 2;
    room[playerY, playerX] = playerSymbol;

    // 적 배치
    Random rand = new Random();
    int enemyCount = currentFloor == 16 ? 1 : rand.Next(1, 5);  // 보스전은 1명 고정
    HashSet<string> used = new HashSet<string> { $"{playerX},{playerY}" };

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

    // 출력
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
    Console.WriteLine($"플레이어 공격력: {player.Atk}, 현재 체력: {player.HP}");

    // ✅ 몬스터 생성
    List<Monster> encountered = new List<Monster>();

    if (currentFloor == 16)
    {
        encountered.Add(new Monster("큐브의 심판자", 50, 300, 25));
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\n⚠ 최종 보스 '큐브의 심판자'가 등장했습니다! ⚠\n");
        Console.ResetColor();
    }
    else
    {
        List<Monster> pool = new List<Monster>
        {
            new Monster("큐브 데몬", 1, 10, 5),
            new Monster("미궁의 기사", 1, 15, 10),
            new Monster("예언자", 1, 5, 13)
        };

        for (int i = 0; i < enemyCount; i++)
        {
            int index = rand.Next(pool.Count);
            Monster baseMonster = pool[index];

            // 층수 기반 능력치 강화
            int level = baseMonster.Level + (currentFloor / 2);
            int hp = baseMonster.HP + (currentFloor * 3);
            int atk = baseMonster.Attack + (currentFloor / 2);

            encountered.Add(new Monster(baseMonster.Name, level, hp, atk));
        }
    }
        // 전투 반복 루프
        while (true)
        {

            foreach (var m in encountered)      //      몬스터 리스트를 하나씩 확인하며 출력
            {
                Console.WriteLine(m.ToString());
            }

            Console.WriteLine();        //      한줄 띄우기 용 <공백 칸>

            Console.WriteLine("\n1. 공격");
            Console.WriteLine("\n원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            string input = Console.ReadLine();

            if (input == "1")
            {
                BattleSystem.PlayerAttack(player, encountered);
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다");
                continue;
            }

            // 승패 조건 확인 시스템
            if (player.HP <= 0 || encountered.All(m => m.HP <= 0))
                break;

            BattleSystem.EnemyPhase(player, encountered);        //      몬스터 턴

            if (player.HP <= 0 || encountered.All(m => m.HP <= 0))
                break;
        }

        // ✅ 경험치 지급
        int totalExp = encountered.Count * 30 + (currentFloor == 16 ? 100 : 0);
        player.GainExp(totalExp);
        Console.WriteLine("\n>> Enter를 누르면 계속 진행합니다...");
        Console.ReadLine();




        Console.WriteLine($"\n출현한 적 수: {enemyCount}");
        Console.WriteLine($"플레이어 공격력: {playerAtk}, 현재 체력: {playerHP}");

        player.GainExp(enemyCount * 30);

        BattleSystem.ShowBattleResult(player, encountered);
    }
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
        int baseDamage = player.TotalAtk;     //      플레이어 기본 공격력 <상태창 완성 시 수정>
        double variance = Math.Ceiling(baseDamage * 0.1);       //      오차가 소수점일 시 올림 처리
        int min = baseDamage - (int)variance;       //      variance = 편차, 즉 - 1 + 1 variance 를 넣음으로 9~11 사이 랜덤 값이 결정됨
        int max = baseDamage + (int)variance;       //      variance = 편차, 즉 - 1 + 1 variance 를 넣음으로 9~11 사이 랜덤 값이 결정됨

        int finalDamage = new Random().Next(min, max + 1);      //      최종 데미지는 = 하한값과 상한값 중 랜덤. max + 1은 값 11까지 나타내기 위해

        // 캐릭터 크리티컬 스탯 반영
        Random rand = new Random();
        bool isCritical = rand.Next(100) < (int)(player.CritChance * 100);      //      플레이어 크리티컬 찬스 반영

        if (isCritical)
        {
            finalDamage = (int)Math.Round(finalDamage * player.CritMultiplier);
            Console.WriteLine("치명타 공격!!");
        }
        //

        int beforeHP = target.HP;

        target.HP -= finalDamage;       //      몬스터 체력에 최종 데미지 -
        if (target.HP <= 0) target.HP = 0;      //      만약 몬스터 체력 0보다 작거나 같다 = 몬스터 체력 0

        //  계산 결과 출력
        Console.WriteLine($"{player.Name} 의 공격!");
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
            int beforeHP = player.HP;

            player.HP -= finalDamage;        //      플레이어 현재 체력에 몬스터의 최종 데미지 -
            if (player.HP <= 0) player.HP = 0;

            Console.WriteLine($"{player.Name}을 맞췄습니다. [데미지 : {finalDamage}]\n");

            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {beforeHP} -> {player.HP}\n");        //      플레이어 몬스터에게 피격 전 체력 -> 피격 후 체력

            Console.ReadLine();
            Console.Clear();
        }
        
        if (player.HP <= 0)
        {
            ShowBattleResult(player, monsters);
        }

        Console.WriteLine("EnemyPhase Phase 종료. 플레이어 턴으로 돌아갑니다.");
        Console.WriteLine("0. 다음");
        Console.ReadLine();
    }

    // 전투 결과 출력 시스템
    public static void ShowBattleResult(Player player, List<Monster> monsters)
    {
        Console.Clear();
        Console.WriteLine("Battle - Result\n");

        if (player.HP <= 0)      //      만약 플레이어 체력이 0보다 작거나 같으면 밑 콘솔 출력
        {
            Console.WriteLine("You Lose\n");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");

            Environment.Exit(0);
            Console.ReadLine();
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
        }
        Console.WriteLine("0 다음");
        Console.WriteLine(">> ");
        Console.ReadLine();
    }

    
}

